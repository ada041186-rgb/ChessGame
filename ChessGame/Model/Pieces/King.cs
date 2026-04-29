using ChessGame.Model;
using ChessGame.Model.Moves;

namespace ChessGame
{
    public class King : Piece
    {
        public override PieceType Type => PieceType.King;
        public override Player Color { get; }
        private static readonly Direction[] dirs = new Direction[]
        {
            Direction.North,
            Direction.South,
            Direction.West,
            Direction.East,
            Direction.NorthEast,
            Direction.NorthWest,
            Direction.SouthEast,
            Direction.SouthWest,
        };
        public King(Player player)
        {
            Color = player;
        }

        public override Piece Copy()
        {
            Piece copy = new King(Color);
            copy.HasMoved = this.HasMoved;
            return copy;
        }

        public IEnumerable<Position> MovePositions(Position from, IBoard board)
        {
            foreach (Direction dir in dirs)
            {
                Position to = from + dir;

                if (!board.IsInside(to))
                {
                    continue;
                }

                if (board.IsEmpty(to) || board[to].Color != Color)
                {
                    yield return to;
                }

            }
        }

        public override IEnumerable<Move> GetMoves(Position from, IBoard board)
        {
            foreach (Position to in MovePositions(from, board))
            {
                yield return new NormalMove(from, to);
            }
        }

        public override bool CanCaptureOpponentKing(Position from, IBoard board)
        {
            return MovePositions(from, board).Any(to =>
            {
                Piece piece = board[to];
                return piece != null && piece.Type == PieceType.King;
            });
        }
    }
}