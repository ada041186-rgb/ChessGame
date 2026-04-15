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
    }
}