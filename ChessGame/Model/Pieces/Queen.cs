using ChessGame.Model;
using ChessGame.Model.Moves;

namespace ChessGame
{
    public class Queen : Piece
    {
        public override PieceType Type => PieceType.Queen;
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
        public Queen(Player player)
        {
            Color = player;
        }

        public override Piece Copy()
        {
            Piece copy = new Queen(Color);
            copy.HasMoved = this.HasMoved;
            return copy;
        }
        public override IEnumerable<Move> GetMoves(Position from, Board board)
        {
            return MovePositionInDirs(from, board, dirs).Select(to => new NormalMove(from, to));
        }

    }
}