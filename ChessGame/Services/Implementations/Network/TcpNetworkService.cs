using ChessGame.Model;
using ChessGame.Model.DTO;
using ChessGame.Model.Moves;
using ChessGame.Services.Interfaces;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.Windows;

namespace ChessGame.Services.Implementations
{
    public class TcpNetworkService : INetworkService
    {
        private readonly IDtoResolver _resolver;
        private readonly IMessageDispatcher _dispatcher;

        private TcpListener tcpListener;
        private TcpClient tcpClient;

        private StreamReader reader;
        private StreamWriter writer;

        private CancellationTokenSource _cts;

        private Task _listenTask;
        private bool _isListening;

        public TcpNetworkService(IDtoResolver resolver, IMessageDispatcher dispatcher)
        {
            _resolver = resolver;
            _dispatcher = dispatcher;
        }


        public async Task StartServerAsync(int port)
        {
            try
            {
                tcpListener?.Stop();

                tcpListener = new TcpListener(IPAddress.Any, port);
                tcpListener.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

                tcpListener.Start();

                tcpClient = await tcpListener.AcceptTcpClientAsync();
                InitStreams();
                StartListening();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[SERVER START ERROR]: {ex.Message}");
                throw;
            }
        }

        public async Task ConnectAsync(string ip, int port)
        {
            tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(ip, port);

            InitStreams();
            StartListening();
        }

        private void InitStreams()
        {
            var stream = tcpClient.GetStream();
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream) { AutoFlush = true };
        }

        private void StartListening()
        {
            if (_isListening) return;

            _isListening = true;
            _cts = new CancellationTokenSource();

            _listenTask = Task.Run(async () =>
            {
                try
                {
                    while (!_cts.IsCancellationRequested)
                    {
                        var raw = await reader.ReadLineAsync();
                        if (raw == null) break;

                        var msg = JsonSerializer.Deserialize<NetworkMessage>(raw);
                        if (msg == null) continue;

                        var dto = _resolver.Deserialize(msg);
                        await _dispatcher.DispatchAsync(dto);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Network Error] {ex}");
                }
                finally
                {
                    _isListening = false;
                }
            });
        }
        public async Task SendAsync(DtoType type, IDtoMessage message)
        {
            if (writer == null)
            {
                return;
            }

            try
            {
                var payload = JsonSerializer.Serialize(message, message.GetType());

                var envelope = new NetworkMessage
                {
                    DtoType = type,
                    Payload = payload
                };

                var json = JsonSerializer.Serialize(envelope);

                await writer.WriteLineAsync(json);
                await writer.FlushAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[SEND ERROR]: {ex.Message}");
            }
        }

        public async Task DisconnectAsync()
        {
            try
            {
                _cts?.Cancel();

                reader?.Dispose();
                writer?.Dispose();

                tcpClient?.Close();
                tcpListener?.Stop();

                if (_listenTask != null)
                    await _listenTask;

                _cts = null;
                _listenTask = null;
            }
            catch { }
        }
    }
}