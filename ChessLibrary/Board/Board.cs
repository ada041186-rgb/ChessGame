using ChessLibrary.Enums;
using ChessLibrary.Pieces;
using ChessLibrary.ValueObjects;
using System.Text;

namespace ChessLibrary.Board
{
    public class Board : IBoard
    {
        public const int Size = 8;
        private readonly IPiece[,] pieces = new IPiece[Size, Size];
        private readonly Dictionary<Player, Position> pawnSkipPositions = new Dictionary<Player, Position>
        {
            [Player.White] = null,
            [Player.Black] = null,
        };
        private readonly IPieceCounterStrategy _pieceCounter;

        public Board(IPieceCounterStrategy pieceCounter = null)
        {
            _pieceCounter = pieceCounter ?? new StandardPieceCounter();
        }
        public IPiece this[int row, int col]
        {
            get { return pieces[row, col]; }
            set { pieces[row, col] = value; }
        }

        public IPiece this[Position pos]
        {
            get { return pieces[pos.Row, pos.Column]; }
            set { pieces[pos.Row, pos.Column] = value; }
        }
        public Position GetPawnSkipPosition(Player player)
        {
            return pawnSkipPositions[player];
        }

        public void SetPawnSkipPosition(Player player, Position pos)
        {
            pawnSkipPositions[player] = pos;
        }
        public bool IsInside(Position pos)
        {
            return pos.Row >= 0 && pos.Row < Size && pos.Column >= 0 && pos.Column < Size;
        }

        public bool IsEmpty(Position pos)
        {
            return this[pos] == null;
        }

        public IEnumerable<Position> PiecePositions()
        {
            for (int r = 0; r < Size; r++)
            {
                for (int c = 0; c < Size; c++)
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
            return PiecePositions()
                .Where(pos => this[pos] != null && this[pos].Color == player);
        }

        public Position FindKing(Player player)
        {
            foreach (var pos in PiecePositionsFor(player))
            {
                var piece = this[pos];
                if (piece?.Type == PieceType.King)
                    return pos;
            }

            return null;
        }

        public IBoard Copy()
        {
            Board copy = new Board(_pieceCounter);
            for (int r = 0; r < Size; r++)
            {
                for (int c = 0; c < Size; c++)
                {
                    if (pieces[r, c] != null)
                    {
                        copy.pieces[r, c] = pieces[r, c].Copy();
                    }
                }
            }

            copy.pawnSkipPositions[Player.White] = pawnSkipPositions[Player.White];
            copy.pawnSkipPositions[Player.Black] = pawnSkipPositions[Player.Black];

            return copy;
        }

        public string GeneratePositionHash()
        {
            var sb = new StringBuilder();
            for (int r = 0; r < Size; r++)
            {
                for (int c = 0; c < Size; c++)
                {
                    IPiece p = pieces[r, c];
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

        public ICountingPieces CountPieces()
        {
            return _pieceCounter.Count(this);
        }
    }
}