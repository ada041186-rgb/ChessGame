using ChessApplication.DTO;
using ChessApplication.DTO.Messages;
using ChessApplication.Interfaces.Game;
using ChessApplication.Interfaces.Utils;
using ChessLibrary.Enums;
using ChessLibrary.Game;

namespace ChessInfrastructure.DTO.Handlers
{
    public class ResignHandler : IMessageHandler<DtoResign>
    {
        private readonly IGameService _gameService;

        public ResignHandler(IGameService gameService)
        {
            _gameService = gameService;
        }

        public Task HandleAsync(DtoResign message)
        {
            var winner = _gameService.ThisPlayer;
            GameResult result = new GameResult(winner, EndGameTypes.Resign);
            _gameService.ForceEndGame(result);
            return Task.CompletedTask;
        }
    }
}
