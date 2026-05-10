using ChessLibrary.Board;
using ChessLibrary.Enums;
using ChessLibrary.Moves.KingMoves;
using ChessLibrary.Pieces;
using ChessLibrary.ValueObjects;

namespace ChessLibrary.Moves.Strategies.KingStrategies
{
    public class KingCastleStrategy : MoveStrategyBase<King>
    {
        protected override IEnumerable<Move> GetMoves(Position from, IBoard board, King king)
        {
            if (king.HasMoved)
                yield break;

            if (CanCastle(board, king, from, Direction.East, 3))
            {
                yield return new CastleMoveKS(from);
            }

            if (CanCastle(board, king, from, Direction.West, 4))
            {
                yield return new CastleMoveQS(from);
            }
        }
        private bool CanCastle(IBoard board, King king, Position kingPos,
            Direction dir, int squaresToRook)
        {
            var current = kingPos + dir;
            for (int i = 1; i < squaresToRook; i++)
            {
                if (!board.IsInside(current) || !board.IsEmpty(current))
                    return false;
                current += dir;
            }

            if (!board.IsInside(current))
                return false;

            var piece = board[current];
            return piece != null
                && piece.Type == PieceType.Rook
                && piece.Color == king.Color
                && !piece.HasMoved;
        }
    }
}