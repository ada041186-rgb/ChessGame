using ChessGame.Model.Enums;
using ChessGame.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Model.Rules
{
    public class CheckmateRule : IEndGameRule
    {
        private readonly IChessRulesService _rules;

        public CheckmateRule(IChessRulesService rules)
        {
            _rules = rules;
        }

        public GameResult Check(Board board, Player nextPlayer, IEnumerable<GameStateMemento> history)
        {
            if (IsCheckmate(board, nextPlayer))
            {
                return new GameResult(nextPlayer.Opponent(), EndGameTypes.Checkmate);
            }

            return null;
        }

        private bool IsCheckmate(Board board, Player player)
        {
            if (!_rules.IsInCheck(board, player)) return false;

            return !_rules.HasAnyLegalMoves(board, player);
        }
    }
}
