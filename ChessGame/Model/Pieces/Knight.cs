using ChessGame.Model;
using ChessGame.Model.Moves;

namespace ChessGame
{
    public class Knight : Piece
    {
        public override PieceType Type => PieceType.Knight;
        public override Player Color { get; }

        public Knight(Player player)
        {
            Color = player;
        }

        public override Piece Copy()
        {
            Piece copy = new Knight(Color);
            copy.HasMoved = this.HasMoved;
            return copy;
        }

        private static IEnumerable<Position> PotentialToPositions(Position from)
        {
            foreach (Direction vDir in new Direction[] { Direction.North, Direction.South })
            {
                foreach (Direction hDir in new Direction[] { Direction.West, Direction.East })
                {
                    yield return from + 2 * vDir + hDir;
                    yield return from + 2 * hDir + vDir;
                }
            }
        }

        private IEnumerable<Position> MovePositions(Position from, IBoard board)
        {
            return PotentialToPositions(from).Where(pos => board.IsInside(pos)
                && (board.IsEmpty(pos) || board[pos].Color != Color));
        }

        public override IEnumerable<Move> GetMoves(Position from, IBoard board)
        {
            return MovePositions(from, board).Select(to => new NormalMove(from, to));
        }
    }
}