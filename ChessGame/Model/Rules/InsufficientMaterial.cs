using ChessGame.Model.Enums;
using ChessGame.Model.Interfaces;
using ChessGame.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Model.Rules
{
    public class InsufficientMaterial : IEndGameRule
    {
        private readonly IChessRulesService _rules;

        public InsufficientMaterial(IChessRulesService rules)
        {
            _rules = rules;
        }

        public GameResult Check(Board board, Player nextPlayer, IEnumerable<GameStateMemento> history)
        {
            if (IsDrawByInsufficientMaterial(board, history))
            {
                return new GameResult(Player.None, EndGameTypes.InsufficientMaterial);
            }

            return null;
        }

        private bool IsDrawByInsufficientMaterial(Board board, IEnumerable<GameStateMemento> history)
        {
            ICountringPieces countringPieces = board.CountPieces();

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

        private static bool IsKingVsKing(ICountringPieces countringPieces)
        {
            return countringPieces.TotalCount == 2;
        }

        private static bool IsKingBishopVsKing(ICountringPieces countringPieces)
        {
            return countringPieces.TotalCount == 3 && (
                countringPieces.GetWhitePieces(PieceType.Bishop) == 1 ||
                countringPieces.GetBlackPieces(PieceType.Bishop) == 1);
        }

        private static bool IsKingKnightVsKing(ICountringPieces countringPieces)
        {
            return countringPieces.TotalCount == 3 && (
                countringPieces.GetWhitePieces(PieceType.Knight) == 1 ||
                countringPieces.GetBlackPieces(PieceType.Knight) == 1);
        }

        private static bool IsKingBishopVsKingBishop(Board board, ICountringPieces countringPieces)
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

        private static Position FindPiece(Board board, Player color, PieceType type)
        {
            return board.PiecePositionsFor(color).First(pos => board[pos].Type == type);
        }
    }
}
