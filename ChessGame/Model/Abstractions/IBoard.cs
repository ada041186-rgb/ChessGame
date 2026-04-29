using ChessGame.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Model
{
    public interface IBoard
    {
        public bool IsEmpty(Position pos);
        public IEnumerable<Position> PiecePositions();

        public IEnumerable<Position> PiecePositionsFor(Player player);
        public Position FindKing(Player player);
        public Board Copy();
        public string GeneratePositionHash();
        public ICountringPieces CountPieces();
    }
}