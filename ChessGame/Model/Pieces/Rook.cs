using ChessGame.Model;
using ChessGame.Model.Moves;

namespace ChessGame
{
    public class Rook : Piece
    {
        public override PieceType Type => PieceType.Rook;
        public override Player Color { get; }

        private static readonly Direction[] dirs = new Direction[]
        {
            Direction.North,
            Direction.South,
            Direction.West,
            Direction.East,
        };
        public Rook(Player player)
        {
            Color = player;
        }

        public override Piece Copy()
        {
            Piece copy = new Rook(Color);
            copy.HasMoved = this.HasMoved;
            return copy;
        }
        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            return MovePositionInDirs(from, board, dirs).Select(to => new NormalMove(from, to));
        }
    }
}