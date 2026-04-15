namespace ChessGame
{
    public class Rook : Piece
    {
        public override PieceType Type => PieceType.Rook;
        public override Player Color { get; }

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
    }
}