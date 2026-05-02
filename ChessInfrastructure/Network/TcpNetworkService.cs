using ChessApplication.DTO;
using ChessApplication.Interfaces.Network;
using ChessInfrastructure.NetworkStates.States;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;

public class TcpNetworkService : INetworkService, INetworkContext
{
    private readonly IDtoResolver _resolver;
    private readonly IMessageDispatcher _dispatcher;
    private readonly ILogger<TcpNetworkService> _logger;

    private INetworkState _state;

    private TcpListener _listener;
    private TcpClient _client;

    private StreamReader _reader;
    private StreamWriter _writer;

    private CancellationTokenSource _cts;
    private Task _listenTask;

    private readonly SemaphoreSlim _writeLock = new(1, 1);

    public event Action OnDisconnected;

    public TcpNetworkService(
        IDtoResolver resolver,
        IMessageDispatcher dispatcher,
        ILogger<TcpNetworkService> logger)
    {
        _resolver = resolver;
        _dispatcher = dispatcher;
        _logger = logger;
        _state = new DisconnectedState(this);
    }

    public void SetState(INetworkState newState)
    {
        _logger.LogInformation("STATE → {State}", newState.GetType().Name);
        _state = newState;
    }

    #region state delegations

    public Task<bool> StartServerAsync(int port)
        => _state.StartServerAsync(port);

    public Task<bool> ConnectAsync(string ip, int port)
        => _state.ConnectAsync(ip, port);

    public Task SendAsync(DtoType type, IDtoMessage message)
        => _state.SendAsync(type, message);

    public Task DisconnectAsync()
        => _state.DisconnectAsync();

    public void Dispose()
    {
        _ = DisconnectAsync();
    }
    #endregion
    
    #region iternal implementations
    public async Task<bool> StartServerInternal(int port)
    {
        try
        {
            _listener?.Stop();
            _listener = new TcpListener(IPAddress.Any, port);
            _listener.Start();

            _client = await _listener.AcceptTcpClientAsync();

            InitStreams();

            StartListening();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Server start error");
            return false;
        }
    }

    public async Task<bool> ConnectInternal(string ip, int port)
    {
        try
        {
            _client = new TcpClient();

            var connectTask = _client.ConnectAsync(ip, port);
            var timeoutTask = Task.Delay(5000);

            if (await Task.WhenAny(connectTask, timeoutTask) == timeoutTask)
                return false;

            await connectTask;

            InitStreams();
            StartListening();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Connect error");
            return false;
        }
    }

    public async Task SendInternal(DtoType type, IDtoMessage message)
    {
        var envelope = new NetworkMessage
        {
            DtoType = type,
            Payload = JsonSerializer.SerializeToElement(message, message.GetType())
        };
        _logger.LogInformation("DISPATCH → {Type}", message.GetType().Name);

        var json = JsonSerializer.Serialize(envelope);

        _logger.LogInformation("SEND → {Type} | Size={Size}", type, json.Length);

        await _writeLock.WaitAsync();
        try
        {
            await _writer.WriteLineAsync(json);
        }
        finally
        {
            _writeLock.Release();
        }
    }

    public async Task DisconnectInternal()
    {
        if (_state is DisconnectingState)
            return;

        this.SetState(new DisconnectingState(this));

        try { _cts?.Cancel(); } catch { }
        try { _client?.Close(); } catch { }

        if (_listenTask != null)
        {
            try { await _listenTask; } catch { }
        }

        _reader = null;
        _writer = null;
        _listener?.Stop();

        OnDisconnected?.Invoke();
        SetState(new DisconnectedState(this));
        _logger.LogInformation("Disconnected");
    }

    #endregion

    #region Server message envelope
    private void InitStreams()
    {
        var stream = _client.GetStream();
        _reader = new StreamReader(stream);
        _writer = new StreamWriter(stream) { AutoFlush = true };

        _logger.LogInformation("Streams initialized");
    }

    private void StartListening()
    {
        _cts = new CancellationTokenSource();

        _listenTask = Task.Run(async () =>
        {
            try
            {
                while (!_cts.IsCancellationRequested)
                {
                    var raw = await _reader.ReadLineAsync();
                    if (raw == null) break;

                    _logger.LogInformation("RECV → {Raw}", raw);

                    try
                    {
                        var envelope = JsonSerializer.Deserialize<NetworkMessage>(raw);
                        var message = _resolver.Deserialize(envelope);
                        await _dispatcher.DispatchAsync(message);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to process message");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Listen error");
                await DisconnectInternal();
            }
        });
    }
    #endregion
}