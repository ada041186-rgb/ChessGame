using ChessGame.Model.Abstractions;
using ChessGame.Services.Implementations.Utils.PieceFactories;
using ChessGame.Services.Interfaces.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Model.PromotionStrategies
{
    public class PromoteToBishopStrategy : IPromotionStrategy
    {
        public PieceType PieceType => PieceType.Bishop;
        public Piece CreatePiece(Player color)
        {
            return new Bishop(color);
        }
    }
}
