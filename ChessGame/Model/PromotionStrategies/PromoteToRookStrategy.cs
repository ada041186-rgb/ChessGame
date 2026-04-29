using ChessGame.Model.Abstractions;
using ChessGame.Services.Interfaces.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Model.PromotionStrategies
{
    public class PromoteToRookStrategy : IPromotionStrategy
    {
        public PieceType PieceType => PieceType.Rook;
        public Piece CreatePiece(Player color)
        {
            return new Rook(color);
        }
    }
}
