using ChessGame.Model;

namespace ChessGame
{
    public abstract class Piece
    {
        public abstract PieceType Type { get; }
        public abstract Player Color { get; }
        public bool HasMoved { get; set; } = false;

        public abstract Piece Copy();

        public abstract IEnumerable<Move> GetMoves(Position from, IBoard board);

        protected IEnumerable<Position> MovePositionInDir(Position from, IBoard board, Direction dir)
        {
            for (Position pos = from + dir; board.IsInside(pos); pos += dir)
            {
                if (board.IsEmpty(pos))
                {
                    yield return pos;
                    continue;
                }

                Piece piece = board[pos];

                if (piece.Color != Color)
                {
                    yield return pos;
                }

                yield break;
            }
        }

        protected IEnumerable<Position> MovePositionInDirs(Position from, IBoard board, Direction[] dirs)
        {
            return dirs.SelectMany(dir => MovePositionInDir(from, board, dir));
        }

        public virtual bool CanCaptureOpponentKing(Position from, IBoard board)
        {
            return GetMoves(from, board).Any(move =>
            {
                Piece piece = board[move.ToPos];
                return piece != null && piece.Type == PieceType.King;
            });
        }
    }
}
