using ChessLibrary.Enums;
using ChessLibrary.ValueObjects;

namespace ChessLibrary.Moves.Factories
{
    public interface IMoveFactory
    {
        Move CreateNormalMove(Position from, Position to);
        IEnumerable<Move> CreatePromotionMoves(Position from, Position to, IEnumerable<PieceType> promotionTypes);
    }
}
