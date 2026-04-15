namespace ChessGame
{
    public class Pawn : Piece
    {
        public override PieceType Type => PieceType.Pawn;
        public override Player Color { get; }

        public Pawn(Player player)
        {
            Color = player;
        }

        public override Piece Copy()
        {
            Piece copy = new Pawn(Color);
            copy.HasMoved = this.HasMoved;
            return copy;
        }
    }
}