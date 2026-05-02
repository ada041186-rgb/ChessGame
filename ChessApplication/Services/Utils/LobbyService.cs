using ChessApplication.DTO;
using ChessApplication.DTO.Messages;
using ChessApplication.Interfaces.Game;
using ChessApplication.Interfaces.Network;
using ChessApplication.Interfaces.Utils;
using ChessLibrary.Enums;
using ChessLibrary.Extensions;

namespace ChessApplication.Services.Utils
{
    public class LobbyService : ILobbyService
    {
        private readonly INetworkService _networkService;
        private readonly IGameService _gameService;

        public event Action<bool>? IsConnected;
        public event Action<Player>? GameStarted;

        private const int PORT = 55555;
        private bool _initialized = false;
        private readonly SemaphoreSlim _initLock = new(1, 1);

        public LobbyService(INetworkService networkService, IGameService gameService)
        {
            _networkService = networkService;
            _gameService = gameService;

            _networkService.OnDisconnected += HandleDisconnected;
        }

        private void HandleDisconnected()
        {
            _initialized = false;
            IsConnected?.Invoke(false);
        }

        public async Task<bool> InitializeAsync(LobbyParams lobbyParams)
        {
            bool isHost = lobbyParams.IsHost;
            string ip = lobbyParams.IpAdress;

            await _initLock.WaitAsync();
            try
            {
                if (_initialized)
                    return true;

                bool success;

                if (isHost)
                {
                    success = await _networkService.StartServerAsync(PORT);
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(ip))
                        return false;

                    success = await _networkService.ConnectAsync(ip, PORT);
                }

                _initialized = success;

                IsConnected?.Invoke(success);
                return success;
            }
            finally
            {
                _initLock.Release();
            }
        }

        public async Task StartLanGameAsync(Player hostPlayer)
        {
            var clientPlayer = hostPlayer.Opponent();

            await _networkService.SendAsync(
                DtoType.StartGame,
                new DtoStartGame(clientPlayer)
            );

            InitializeLocalGame(hostPlayer);
        }

        public void HandleLocalStartGame(DtoStartGame message)
        {
            InitializeLocalGame(message.StartingSide);
        }

        private void InitializeLocalGame(Player player)
        {
            _gameService.InitGame(player);
            GameStarted?.Invoke(player);
        }

        public async Task DisconnectAsync()
        {
            await _networkService.DisconnectAsync();
            _initialized = false;
        }

        public void Reset()
        {
            _initialized = false;
        }
    }
}