using ChessLibrary.Board;
using ChessLibrary.Pieces;
using ChessLibrary.ValueObjects;

namespace ChessLibrary.Moves.Strategies
{
    public interface IMoveService
    {
        IEnumerable<Move> GetMoves(IPiece piece, Position from, IBoard board);
    }
}
