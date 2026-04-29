using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Model.Interfaces
{
    public interface ICountringPieces
    {
        int TotalCount { get; }

        public void Increment(Player color, PieceType type);

        public int GetWhitePieces(PieceType type);
        public int GetBlackPieces(PieceType type);
    }
}
