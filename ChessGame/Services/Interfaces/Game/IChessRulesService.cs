using ChessGame.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Services.Interfaces
{
    public interface IChessRulesService
    {
        IEnumerable<Move> GetLegalMoves(Board board, Player player, Position pos);
        public bool HasAnyLegalMoves(Board board, Player player);
        bool IsMoveLegal(Board board, Move move);
        Position GetKingInCheck(Board board);
        bool IsInCheck(Board board, Player player);
    }
}
