using ChessGame.Model;
using ChessGame.Model.DTO.Messages;
using ChessGame.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Services.Implementations
{
    public class LobbyService : ILobbyService
    {
        private readonly INetworkService _networkService;
        private readonly IGameService _gameService;

        public event Action<bool> IsConnected;
        private int _port = 55555;
        public LobbyService(INetworkService networkService, IGameService gameService)
        {
            _networkService = networkService;
            _gameService = gameService;
        }

        public async Task InitializeAsync(bool isHost, string ip = null)
        {
            if (isHost)
            {
                await _networkService.StartServerAsync(_port);
            }
            else
            {
                await _networkService.ConnectAsync(ip, _port);
            }

            IsConnected?.Invoke(true);
        }

        public async Task StartGameAsync()
        {
            await _networkService.SendAsync(
                DtoType.StartGame,
                new DtoStartGame(Player.Black)
            );

            _gameService.InitGame(Player.White);
        }
    }
}
