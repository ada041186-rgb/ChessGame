using ChessGame.Model.Abstractions;
using ChessGame.Model.Interfaces;
using ChessGame.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Model
{
    public class Board : IBoard
    {
        private readonly Piece[,] pieces = new Piece[8,8];
        private readonly IPieceCounterStrategy _pieceCounter;
        public Board(IPieceCounterStrategy pieceCounter = null)
        {
            _pieceCounter = pieceCounter ?? new StandardPieceCounter();
        }

        public Piece this[int row, int col]
        {
            get { return pieces[row, col]; }
            internal set { pieces[row, col] = value; }
        }

        public Piece this[Position pos]
        {
            get { return pieces[pos.Row, pos.Column]; }
            internal set { pieces[pos.Row, pos.Column] = value; }
        }

        public static bool IsInside(Position pos)
        {
            return pos.Row >= 0 && pos.Row < 8 && pos.Column >= 0 && pos.Column < 8;
        }

        public bool IsEmpty(Position pos)
        {
            return this[pos] == null;
        }

        public IEnumerable<Position> PiecePositions()
        {
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    Position pos = new Position(r, c);

                    if (!IsEmpty(pos))
                    {
                        yield return pos;
                    }
                }
            }
        }

        public IEnumerable<Position> PiecePositionsFor(Player player)
        {
            return PiecePositions().Where(pos => this[pos].Color == player);
        }

        public Position FindKing(Player player)
        {
            return PiecePositionsFor(player)
                .FirstOrDefault(pos => this[pos].Type == PieceType.King);
        }
        public Board Copy()
        {
            Board copy = new Board();
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    if (pieces[r, c] != null)
                    {
                        copy.pieces[r, c] = pieces[r, c].Copy();
                    }
                }
            }
            return copy;
        }
        public string GeneratePositionHash()
        {
            var sb = new StringBuilder();
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    Piece p = pieces[r, c];
                    if (p == null)
                    {
                        sb.Append('-');
                    }
                    else
                    {
                        sb.Append(p.Color == Player.White ? "W" : "B");
                        sb.Append((int)p.Type);
                    }
                }
            }
            return sb.ToString();
        }
        public ICountringPieces CountPieces()
        {
            return _pieceCounter.Count(this);
        }
    }
}
