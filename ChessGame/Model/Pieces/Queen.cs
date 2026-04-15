namespace ChessGame
{
    public class Queen : Piece
    {
        public override PieceType Type => PieceType.Queen;
        public override Player Color { get; }

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
    }
}