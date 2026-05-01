using ChessApplication.DTO;
using ChessApplication.Interfaces.Network;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;

namespace ChessInfrastructure.Network
{
    public class TcpNetworkService : INetworkService
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
            try
            {
                _logger.LogInformation("Starting server on port {Port}", port);

                _listener?.Stop();

                _listener = new TcpListener(IPAddress.Any, port);
                _listener.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

                _listener.Start();
                _logger.LogInformation("Server started, waiting for client...");

                _client = await _listener.AcceptTcpClientAsync();
                _logger.LogInformation("Client connected");

                InitStreams();

                if (!await HandshakeAsync(isServer: true))
                {
                    _logger.LogWarning("Handshake failed (server side)");
                    return false;
                }

                StartListening();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while starting server");
                return false;
            }
        }

        public async Task<bool> ConnectAsync(string ip, int port)
        {
            try
            {
                _logger.LogInformation("Connecting to {IP}:{Port}", ip, port);

                _client = new TcpClient();

                var connectTask = _client.ConnectAsync(ip, port);
                var timeoutTask = Task.Delay(5000);

                var completed = await Task.WhenAny(connectTask, timeoutTask);

                if (completed == timeoutTask)
                {
                    _logger.LogWarning("Connection timeout");
                    return false;
                }

                _logger.LogInformation("Connected to server");

                InitStreams();

                if (!await HandshakeAsync(isServer: false))
                {
                    _logger.LogWarning("Handshake failed (client side)");
                    return false;
                }

                StartListening();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while connecting to server");
                return false;
            }
        }

        private async Task<bool> HandshakeAsync(bool isServer)
        {
            try
            {
                _logger.LogInformation("Starting handshake. IsServer={IsServer}", isServer);

                if (isServer)
                {
                    await SendRawAsync("HELLO_SERVER");
                    var response = await _reader.ReadLineAsync();

                    var ok = response == "HELLO_CLIENT";
                    _logger.LogInformation("Handshake response: {Response}, success={Success}", response, ok);

                    return ok;
                }
                else
                {
                    var request = await _reader.ReadLineAsync();

                    if (request != "HELLO_SERVER")
                    {
                        _logger.LogWarning("Invalid handshake request: {Request}", request);
                        return false;
                    }

                    await SendRawAsync("HELLO_CLIENT");
                    _logger.LogInformation("Handshake completed (client)");

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

            _logger.LogInformation("Network streams initialized");
        }

        private void StartListening()
        {
            _cts = new CancellationTokenSource();

            _logger.LogInformation("Started listening loop");

            _listenTask = Task.Run(async () =>
            {
                try
                {
                    while (!_cts.IsCancellationRequested)
                    {
                        string raw;

                        try
                        {
                            raw = await _reader.ReadLineAsync();
                        }
                        catch (IOException ex) when (_cts.IsCancellationRequested)
                        {
                            _logger.LogInformation("Listening stopped (cancelled)");
                            break;
                        }
                        catch (IOException ex)
                        {
                            _logger.LogWarning(ex, "Connection lost (IO exception)");
                            break;
                        }
                        catch (SocketException ex)
                        {
                            _logger.LogWarning(ex, "Connection forcibly closed by remote host");
                            break;
                        }

                        if (raw == null)
                        {
                            _logger.LogInformation("Remote disconnected gracefully");
                            break;
                        }

                        NetworkMessage msg;

                        try
                        {
                            msg = JsonSerializer.Deserialize<NetworkMessage>(raw);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, "Invalid JSON received: {Raw}", raw);
                            continue;
                        }

                        if (msg == null)
                        {
                            _logger.LogWarning("Null message received");
                            continue;
                        }

                        var dto = _resolver.Deserialize(msg);

                        _logger.LogDebug("Received message: {Type}", msg.DtoType);

                        await _dispatcher.DispatchAsync(dto);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Listening loop crashed unexpectedly");
                }
                finally
                {
                    _logger.LogInformation("Listening loop stopped");
                }
            });
        }

        public async Task SendAsync(DtoType type, IDtoMessage message)
        {
            if (_writer == null)
            {
                _logger.LogWarning("SendAsync called but writer is null");
                return;
            }

            var payload = JsonSerializer.Serialize(message, message.GetType());

            var envelope = new NetworkMessage
            {
                DtoType = type,
                Payload = payload
            };

            var json = JsonSerializer.Serialize(envelope);

            await _writer.WriteLineAsync(json);

            _logger.LogDebug("Sent message: {Type}", type);
        }

        private async Task SendRawAsync(string msg)
        {
            await _writer.WriteLineAsync(msg);
            _logger.LogDebug("Sent raw message: {Msg}", msg);
        }

        public async Task DisconnectAsync()
        {
            try
            {
                _logger.LogInformation("Disconnecting network service");

                _cts?.Cancel();

                _reader?.Dispose();
                _writer?.Dispose();

                _client?.Close();
                _listener?.Stop();

                if (_listenTask != null)
                    await _listenTask;

                _logger.LogInformation("Disconnected successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during disconnect");
            }
        }
    }
}