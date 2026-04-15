namespace ChessGame
{
    public class King : Piece
    {
        public override PieceType Type => PieceType.King;
        public override Player Color { get; }

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
    }
}