using ChessGame.Model;
using ChessGame.Model.Moves;

namespace ChessGame
{
    public class Pawn : Piece
    {
        public override PieceType Type => PieceType.Pawn;
        public override Player Color { get; }

        public readonly Direction forward;

        public Pawn(Player player)
        {
            Color = player;

            if (player == Player.White)
            {
                forward = Direction.North;
            }
            else if(player == Player.Black)
            {
                forward = Direction.South;
            }
        }

        public override Piece Copy()
        {
            Piece copy = new Pawn(Color);
            copy.HasMoved = this.HasMoved;
            return copy;
        }

        private static bool CanMoveTo(Position pos, Board board)
        {
            return Board.IsInside(pos) && board.IsEmpty(pos);
        }

        private bool CanCapture(Position pos, Board board)
        {
            if (!Board.IsInside(pos) || board.IsEmpty(pos))
            {
                return false;
            }

            return board[pos].Color != Color;
        }

        private IEnumerable<Move> ForwardMoves(Position from, Board board)
        {
            Position oneMovePos = from + forward;

            if(CanMoveTo(oneMovePos, board))
            {
                yield return new NormalMove(from, oneMovePos);

                Position twoMovesPos = oneMovePos + forward;

                if (!HasMoved && CanMoveTo(twoMovesPos, board))
                {
                    yield return new NormalMove(from, twoMovesPos);
                }
            }
        }


        private IEnumerable<Move> DiagonalMoves(Position from, Board board)
        {
            foreach(Direction dir in new Direction[] { Direction.West, Direction.East })
            {
                Position to = from + forward + dir;

                if (CanCapture(to, board))
                {
                    yield return new NormalMove(from, to);
                }
            }
        }

        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            return ForwardMoves(from, board).Concat(DiagonalMoves(from, board));
        }
    }
}