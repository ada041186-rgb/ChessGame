using ChessApplication.DTO;
using ChessApplication.DTO.Messages;
using ChessApplication.Interfaces.Game;
using ChessApplication.Interfaces.Network;
using ChessLibrary.Enums;
using ChessLibrary.Moves;

namespace ChessInfrastructure.DTO.Factories
{
    public class DtoEnPasssantMoveFactory : ISpecificDtoMoveFactory
    {
        private readonly IGameService _gameService;

        public DtoEnPasssantMoveFactory(IGameService gameService)
        {
            _gameService = gameService;
        }

        public DtoType TargetDtoType => DtoType.EnPassant;
        public MoveType TargetMoveType => MoveType.EnPassant;

        public Move GetMoveFromDTO(IDtoMessage message)
        {
            var dto = (DtoEnPassantMove)message;
            var legalMoves = _gameService.GetLegalMoves(dto.FromPos);
            var localMove = legalMoves.FirstOrDefault(m => m.ToPos == dto.ToPos && m.Type == TargetMoveType);

            return localMove ?? throw new InvalidOperationException("Нелегальний double хід.");
        }

        public IDtoMessage GetMoveToDTO(Move move)
        {
            return new DtoEnPassantMove(move.FromPos, move.ToPos);
        }
    }
}