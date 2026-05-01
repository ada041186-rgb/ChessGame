using ChessApplication.DTO;
using ChessApplication.DTO.Messages;
using ChessApplication.Interfaces.Game;
using ChessApplication.Interfaces.Network;
using ChessLibrary.Enums;
using ChessLibrary.Moves;

namespace ChessInfrastructure.DTO.Factories
{
    public class DtoNormalMoveFactory : ISpecificDtoMoveFactory
    {
        private readonly IGameService _gameService;

        public DtoNormalMoveFactory(IGameService gameService)
        {
            _gameService = gameService;
        }

        public DtoType TargetDtoType => DtoType.NormalMove;
        public MoveType TargetMoveType => MoveType.NormalMove;

        public Move GetMoveFromDTO(IDtoMessage message)
        {
            var dto = (DtoNormalMove)message;
            var legalMoves = _gameService.GetLegalMoves(dto.FromPos);
            var localMove = legalMoves.FirstOrDefault(m => m.ToPos == dto.ToPos && m.Type == MoveType.NormalMove);

            return localMove ?? throw new InvalidOperationException("Нелегальний звичайний хід.");
        }

        public IDtoMessage GetMoveToDTO(Move move)
        {
            return new DtoNormalMove(move.FromPos, move.ToPos);
        }
    }
}
