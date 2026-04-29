using ChessGame.Model.Interfaces;

namespace ChessGame.Model
{
    public interface IBoard
    {
        Piece this[int row, int col] { get; set; }
        Piece this[Position pos] { get; set; }

        bool IsEmpty(Position pos);
        bool IsInside(Position pos);
        IEnumerable<Position> PiecePositions();
        IEnumerable<Position> PiecePositionsFor(Player player);
        Position FindKing(Player player);
        IBoard Copy();
        string GeneratePositionHash();
        ICountingPieces CountPieces();
    }
}