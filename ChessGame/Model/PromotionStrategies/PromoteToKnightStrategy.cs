using ChessGame.Model.Abstractions;
using ChessGame.Services.Interfaces.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Model.PromotionStrategies
{
    public class PromoteToKnightStrategy : IPromotionStrategy
    {
        public PieceType PieceType => PieceType.Knight;
        public Piece CreatePiece(Player color)
        {
            return new Knight(color);
        }
    }
}
