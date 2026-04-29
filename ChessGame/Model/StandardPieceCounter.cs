using ChessGame.Model.Abstractions;
using ChessGame.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Model
{
    public class StandardPieceCounter : IPieceCounterStrategy
    {
        public ICountringPieces Count(Board board)
        {
            var counter = new CountingPieces();

            foreach (Position pos in board.PiecePositions())
            {
                Piece piece = board[pos];
                counter.Increment(piece.Color, piece.Type);
            }

            return counter;
        }
    }
}
