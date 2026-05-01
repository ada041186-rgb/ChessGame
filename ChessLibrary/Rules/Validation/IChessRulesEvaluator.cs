using ChessLibrary.Board;
using ChessLibrary.Enums;
using ChessLibrary.Moves;
using ChessLibrary.ValueObjects;

namespace ChessLibrary.Rules.Validation
{
    public interface IChessRulesEvaluator
    {
        bool IsMoveLegal(IBoard board, Move move);
        Position GetKingInCheck(IBoard board);
        bool IsInCheck(IBoard board, Player player);
        bool HasAnyLegalMoves(IBoard board, Player player);
        IEnumerable<Move> GetLegalMoves(IBoard board, Player player, Position pos);
    }
}
