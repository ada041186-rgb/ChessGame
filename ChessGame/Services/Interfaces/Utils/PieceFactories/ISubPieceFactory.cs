using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Services.Interfaces.Utils.PieceFactories
{
    public interface ISubPieceFactory
    {
        PieceType Type { get; }
        Piece Create(Player color);
    }
}
