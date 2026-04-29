using ChessGame.Model.Enums;
using ChessGame.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Model.Rules
{
    public class StalemateRule : IEndGameRule
    {
        private readonly IChessRulesService _rules;

        public StalemateRule(IChessRulesService rules)
        {
            _rules = rules;
        }

        public GameResult Check(Board board, Player nextPlayer, IEnumerable<GameStateMemento> history)
        {
            if (IsStalemate(board, nextPlayer))
            {
                return new GameResult(Player.None, EndGameTypes.Stalemate);
            }

            return null;
        }

        private bool IsStalemate(Board board, Player player)
        {
            if (_rules.IsInCheck(board, player)) return false;

            return !_rules.HasAnyLegalMoves(board, player);
        }
    }
}
