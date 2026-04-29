using ChessGame.Model;
using ChessGame.Model.DTO.Messages;
using ChessGame.Model.Moves;
using ChessGame.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Services.Implementations.Network
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

            var localMove = legalMoves
                .OfType<PawnPromotion>()
                .FirstOrDefault(m => m.ToPos == dto.ToPos &&
                                     m.PromotionStrategy.PieceType == dto.PromotionPieceType);

            return localMove;
        }

        public IDtoMessage GetMoveToDTO(Move move)
        {
            var promotion = (PawnPromotion)move;
            return new DtoPromotionMove(promotion.FromPos, promotion.ToPos, promotion.PromotionStrategy.PieceType);
        }
    }
}