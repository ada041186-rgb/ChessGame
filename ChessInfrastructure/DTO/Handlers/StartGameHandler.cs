using ChessApplication.DTO;
using ChessApplication.DTO.Messages;
using ChessApplication.Interfaces.Game;
using ChessApplication.Interfaces.Utils;

namespace ChessInfrastructure.DTO.Handlers
{
    public class StartGameHandler : IMessageHandler<DtoStartGame>
    {
        private readonly IGameService _gameService;
        private readonly ILobbyService _lobbyService;

        public StartGameHandler(IGameService gameService, ILobbyService lobbyService)
        {
            _gameService = gameService;
            _lobbyService = lobbyService;
        }

        public Task HandleAsync(DtoStartGame message)
        {
            _lobbyService.HandleLocalStartGame(message);

            return Task.CompletedTask;
        }
    }
}
