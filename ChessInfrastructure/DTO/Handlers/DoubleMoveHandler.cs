using ChessApplication.DTO;
using ChessApplication.DTO.Messages;
using ChessApplication.Interfaces.Game;
using ChessApplication.Interfaces.Network;


namespace ChessInfrastructure.DTO.Handlers
{
    public class DoubleMoveHandler : IMessageHandler<DtoDoubleMove>
    {
        private readonly IGameService _gameService;
        private readonly IDtoMoveFactory _moveFactory;

        public DoubleMoveHandler(IDtoMoveFactory moveFactory, IGameService gameService)
        {
            _moveFactory = moveFactory;
            _gameService = gameService;
        }

        public Task HandleAsync(DtoDoubleMove message)
        {
            var move = _moveFactory.GetMoveFromDTO(message);
            _gameService.TryMakeMove(move);
            return Task.CompletedTask;
        }
    }
}
