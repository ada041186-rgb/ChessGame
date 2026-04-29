using ChessGame.Model.DTO.Messages;
using ChessGame.Model.Moves;
using ChessGame.Services.Interfaces;
using System.Windows;


namespace ChessGame.Model.DTO.Handlers
{
    public class PromotionMoveHandler : IMessageHandler<DtoPromotionMove>
    {
        private readonly IGameService _gameService;
        private readonly IDtoMoveFactory _moveFactory;

        public PromotionMoveHandler(IDtoMoveFactory moveFactory, IGameService gameService)
        {
            _moveFactory = moveFactory;
            _gameService = gameService;
        }

        public Task HandleAsync(DtoPromotionMove message)
        {
            var move = _moveFactory.GetMoveFromDTO(message);
            _gameService.TryMakeMove(move);
            return Task.CompletedTask;
        }
    }
}
