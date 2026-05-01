using ChessApplication.DTO;
using ChessApplication.Interfaces.Network;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;

namespace ChessInfrastructure.Network
{
    public class TcpNetworkService : INetworkService, IDisposable
    {
        private readonly IDtoResolver _resolver;
        private readonly IMessageDispatcher _dispatcher;
        private readonly ILogger<TcpNetworkService> _logger;

        private TcpListener _listener;
        private TcpClient _client;

        private StreamReader _reader;
        private StreamWriter _writer;

        private CancellationTokenSource _cts;
        private Task _listenTask;

        private volatile bool _isConnected = false;
        private bool _disposed = false;
        private int _disconnectNotified = 0;

        private readonly SemaphoreSlim _writeLock = new SemaphoreSlim(1, 1);

        public event Action OnDisconnected;

        public TcpNetworkService(
            IDtoResolver resolver,
            IMessageDispatcher dispatcher,
            ILogger<TcpNetworkService> logger)
        {
            _resolver = resolver;
            _dispatcher = dispatcher;
            _logger = logger;
        }


        public async Task<bool> StartServerAsync(int port)
        {
            if (_disposed) return false;

            try
            {
                _logger.LogInformation("Starting server on port {Port}", port);

                _listener?.Stop();
                _listener = new TcpListener(IPAddress.Any, port);
                _listener.Start();

                _client = await _listener.AcceptTcpClientAsync();

                InitStreams();

                if (!await HandshakeAsync(true))
                    return false;

                StartListening();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Server start error");
                return false;
            }
        }


        public async Task<bool> ConnectAsync(string ip, int port)
        {
            if (_disposed) return false;

            try
            {
                _client = new TcpClient();

                var connectTask = _client.ConnectAsync(ip, port);
                var timeoutTask = Task.Delay(5000);

                if (await Task.WhenAny(connectTask, timeoutTask) == timeoutTask)
                    return false;

                await connectTask;

                InitStreams();

                if (!await HandshakeAsync(false))
                    return false;

                StartListening();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Connect error");
                return false;
            }
        }


        private async Task<bool> HandshakeAsync(bool isServer)
        {
            try
            {
                if (isServer)
                {
                    await SendRawAsync("HELLO_SERVER");
                    var response = await _reader.ReadLineAsync();
                    return response == "HELLO_CLIENT";
                }
                else
                {
                    var request = await _reader.ReadLineAsync();
                    if (request != "HELLO_SERVER")
                        return false;

                    await SendRawAsync("HELLO_CLIENT");
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Handshake error");
                return false;
            }
        }


        private void InitStreams()
        {
            var stream = _client.GetStream();
            _reader = new StreamReader(stream);
            _writer = new StreamWriter(stream) { AutoFlush = true };

            _isConnected = true;
            _disconnectNotified = 0;

            _logger.LogInformation("Streams initialized");
        }


        private void StartListening()
        {
            _cts = new CancellationTokenSource();

            _listenTask = Task.Run(async () =>
            {
                try
                {
                    while (!_cts.IsCancellationRequested && _isConnected)
                    {
                        var raw = await _reader.ReadLineAsync();

                        if (raw == null)
                        {
                            await Task.Delay(50);

                            if (!_isConnected)
                                break;

                            continue;
                        }

                        var msg = JsonSerializer.Deserialize<NetworkMessage>(raw);
                        if (msg == null) continue;

                        var dto = _resolver.Deserialize(msg);
                        await _dispatcher.DispatchAsync(dto);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Listen loop error");
                    SafeDisconnect();
                }
            });
        }


        public async Task SendAsync(DtoType type, IDtoMessage message)
        {
            if (!_isConnected || _disposed) return;

            var envelope = new NetworkMessage
            {
                DtoType = type,
                Payload = JsonSerializer.SerializeToElement(message, message.GetType())
            };

            var json = JsonSerializer.Serialize(envelope);

            _logger.LogInformation(
                "SEND → {Type} | Size={Size} bytes | Payload={Payload}",
                type,
                json.Length,
                json
            );

            await _writeLock.WaitAsync();
            try
            {
                if (!_isConnected) return;
                await _writer.WriteLineAsync(json);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Send failed");
                SafeDisconnect();
            }
            finally
            {
                _writeLock.Release();
            }
        }

        private async Task SendRawAsync(string msg)
        {
            if (_disposed) return;

            await _writeLock.WaitAsync();
            try
            {
                if (!_isConnected) return;
                await _writer.WriteLineAsync(msg);
            }
            finally
            {
                _writeLock.Release();
            }
        }


        private void SafeDisconnect()
        {
            if (Interlocked.Exchange(ref _disconnectNotified, 1) == 1)
                return;

            _isConnected = false;

            try { _cts?.Cancel(); } catch { }
            try { _client?.Close(); } catch { }

            OnDisconnected?.Invoke();

            _logger.LogInformation("Safe disconnect done");
        }


        public async Task DisconnectAsync()
        {
            if (_disposed) return;

            SafeDisconnect();

            if (_listenTask != null)
            {
                try { await _listenTask; }
                catch { }
            }

            _reader = null;
            _writer = null;

            _listener?.Stop();

            _logger.LogInformation("Disconnected (service still alive)");
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            try { _cts?.Cancel(); } catch { }

            try { _reader?.Dispose(); } catch { }
            try { _writer?.Dispose(); } catch { }

            try { _writeLock?.Dispose(); } catch { }

            try { _client?.Dispose(); } catch { }
            try { _listener?.Stop(); } catch { }

            _logger.LogInformation("TcpNetworkService disposed");
        }
    }
}