using ChessGame.Model.Abstractions;
using ChessGame.Services.Implementations.Utils.PieceFactories;
using ChessGame.Services.Interfaces.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Model.Moves
{
    public class PawnPromotion : Move
    {
        public override MoveType Type => MoveType.PawnPromotion;

        public override Position FromPos { get; }

        public override Position ToPos {  get; }

        public IPromotionStrategy PromotionStrategy { get; }

        public PawnPromotion(Position fromPos, Position toPos, IPromotionStrategy promotionStrategy)
        {
            FromPos = fromPos;
            ToPos = toPos;
            PromotionStrategy = promotionStrategy;
        }

        public override void Execute(IBoard board)
        {
            Piece pawn = board[FromPos];
            Player playerColor = pawn.Color;
            board[FromPos] = null;
            board[ToPos] = PromotionStrategy.CreatePiece(playerColor);
        }
    }
}
