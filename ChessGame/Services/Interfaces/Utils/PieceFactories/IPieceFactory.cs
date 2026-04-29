using ChessGame.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Services.Interfaces.Utils
{
    public interface IPieceFactory
    {
        Piece CreatePiece(PieceType type, Player color);
    }
}
