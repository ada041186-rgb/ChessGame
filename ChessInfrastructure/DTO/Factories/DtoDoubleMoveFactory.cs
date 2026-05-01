using ChessApplication.DTO;
using ChessApplication.DTO.Messages;
using ChessApplication.Interfaces.Game;
using ChessApplication.Interfaces.Network;
using ChessLibrary.Enums;
using ChessLibrary.Moves;

namespace ChessInfrastructure.DTO.Factories
{
    public class DtoDoubleMoveFactory : ISpecificDtoMoveFactory
    {
        private readonly IGameService _gameService;

        public DtoDoubleMoveFactory(IGameService gameService)
        {
            _gameService = gameService;
        }

        public DtoType TargetDtoType => DtoType.DoubleMove;
        public MoveType TargetMoveType => MoveType.DoublePawn;

        public Move GetMoveFromDTO(IDtoMessage message)
        {
            var dto = (DtoDoubleMove)message;
            var legalMoves = _gameService.GetLegalMoves(dto.FromPos);
            var localMove = legalMoves.FirstOrDefault(m => m.ToPos == dto.ToPos && m.Type == MoveType.DoublePawn);

            return localMove ?? throw new InvalidOperationException("Нелегальний double хід.");
        }

        public IDtoMessage GetMoveToDTO(Move move)
        {
            return new DtoDoubleMove(move.FromPos, move.ToPos);
        }
    }
}