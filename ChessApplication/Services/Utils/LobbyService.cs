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

        private const int _port = 55555;

        public LobbyService(INetworkService networkService, IGameService gameService)
        {
            _networkService = networkService;
            _gameService = gameService;
        }

        public void HandleLocalStartGame(DtoStartGame message)
        {
            InitializeLocalGame(message.StartingSide);
        }

        public async Task StartLanGameAsync(Player player)
        {
            await _networkService.SendAsync(
                DtoType.StartGame,
                new DtoStartGame(player.Opponent())
            );

            InitializeLocalGame(player);
        }

        private void InitializeLocalGame(Player player)
        {
            _gameService.InitGame(player);
            GameStarted?.Invoke(player);
        }

        public async Task<bool> InitializeAsync(LobbyParams lobbyParams)
        {
            bool success;
            bool isHost = lobbyParams.IsHost;
            string ip = lobbyParams.IpAdress;

            if (isHost)
            {
                success = await _networkService.StartServerAsync(_port);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(ip))
                    return false;

                success = await _networkService.ConnectAsync(ip, _port);
            }

            IsConnected?.Invoke(success);

            return success;
        }
    }
}