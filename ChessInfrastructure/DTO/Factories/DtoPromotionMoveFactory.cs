using ChessApplication.DTO;
using ChessApplication.DTO.Messages;
using ChessApplication.Interfaces.Game;
using ChessApplication.Interfaces.Network;
using ChessLibrary.Enums;
using ChessLibrary.Moves;
using ChessLibrary.Moves.PawnMoves;

namespace ChessInfrastructure.DTO.Factories
{
    public class DtoPromotionMoveFactory : ISpecificDtoMoveFactory
    {
        private readonly IGameService _gameService;

        public DtoPromotionMoveFactory(IGameService gameService)
        {
            _gameService = gameService;
        }

        public DtoType TargetDtoType => DtoType.PromotionMove;
        public MoveType TargetMoveType => MoveType.PawnPromotion;

        public Move GetMoveFromDTO(IDtoMessage message)
        {
            var dto = (DtoPromotionMove)message;

            var legalMoves = _gameService.GetLegalMoves(dto.FromPos);

            return legalMoves
                .OfType<PawnPromotion>()
                .FirstOrDefault(m =>
                    m.ToPos == dto.ToPos &&
                    m.PromotionPieceType == dto.PromotionPieceType);
        }

        public IDtoMessage GetMoveToDTO(Move move)
        {
            var promotion = (PawnPromotion)move;

            return new DtoPromotionMove(
                promotion.FromPos,
                promotion.ToPos,
                promotion.PromotionPieceType);
        }
    }
}