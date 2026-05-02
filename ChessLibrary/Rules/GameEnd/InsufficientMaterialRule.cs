using ChessLibrary.Board;
using ChessLibrary.Enums;
using ChessLibrary.Game;
using ChessLibrary.ValueObjects;

namespace ChessLibrary.Rules.GameEnd
{
    public class InsufficientMaterialRule : EndGameRuleHandler
    {
        public override GameResult? Check(IBoard board, Player nextPlayer, IEnumerable<GameStateMemento> history)
        {
            if (IsDrawByInsufficientMaterial(board, history))
            {
                return new GameResult(Player.None, EndGameTypes.InsufficientMaterial);
            }

            return base.Check(board, nextPlayer, history);
        }

        private bool IsDrawByInsufficientMaterial(IBoard board, IEnumerable<GameStateMemento> history)
        {
            ICountingPieces countringPieces = board.CountPieces();

            if (IsKingVsKing(countringPieces))
            {
                return true;
            }

            if (IsKingBishopVsKing(countringPieces))
            {
                return true;
            }

            if (IsKingKnightVsKing(countringPieces))
            {
                return true;
            }

            if (IsKingBishopVsKingBishop(board, countringPieces))
            {
                return true;
            }

            return false;
        }

        private static bool IsKingVsKing(ICountingPieces countringPieces)
        {
            return countringPieces.TotalCount == 2;
        }

        private static bool IsKingBishopVsKing(ICountingPieces countringPieces)
        {
            return countringPieces.TotalCount == 3 && (
                countringPieces.GetWhitePieces(PieceType.Bishop) == 1 ||
                countringPieces.GetBlackPieces(PieceType.Bishop) == 1);
        }

        private static bool IsKingKnightVsKing(ICountingPieces countringPieces)
        {
            return countringPieces.TotalCount == 3 && (
                countringPieces.GetWhitePieces(PieceType.Knight) == 1 ||
                countringPieces.GetBlackPieces(PieceType.Knight) == 1);
        }

        private static bool IsKingBishopVsKingBishop(IBoard board, ICountingPieces countringPieces)
        {
            if (countringPieces.TotalCount != 4)
            {
                return false;
            }

            if (countringPieces.GetWhitePieces(PieceType.Bishop) != 1 || countringPieces.GetBlackPieces(PieceType.Bishop) != 1)
            {
                return false;
            }

            Position wBishopPos = FindPiece(board, Player.White, PieceType.Bishop);
            Position bBishopPos = FindPiece(board, Player.Black, PieceType.Bishop);

            return wBishopPos.SquareColor() == bBishopPos.SquareColor();
        }

        private static Position FindPiece(IBoard board, Player color, PieceType type)
        {
            return board.PiecePositionsFor(color).First(pos => board[pos].Type == type);
        }
    }
}
