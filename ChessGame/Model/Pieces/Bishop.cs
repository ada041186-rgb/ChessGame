namespace ChessGame
{
    public class Bishop : Piece
    {
        public override PieceType Type => PieceType.Bishop;
        public override Player Color { get; }

        public Bishop(Player player)
        {
            Color = player;
        }

        public override Piece Copy()
        {
            Piece copy = new Bishop(Color);
            copy.HasMoved = this.HasMoved;
            return copy;
        }
    }
}