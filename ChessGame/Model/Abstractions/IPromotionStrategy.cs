using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Model.Abstractions
{
    public interface IPromotionStrategy
    {
        PieceType PieceType { get; }
        Piece CreatePiece(Player color);
    }
}
