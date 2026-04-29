using ChessGame.Model.Enums;
using ChessGame.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Model.Rules
{
    public class RepetitionRule : IEndGameRule
    {
        private readonly IChessRulesService _rules;

        public RepetitionRule(IChessRulesService rules)
        {
            _rules = rules;
        }

        public GameResult Check(Board board, Player nextPlayer, IEnumerable<GameStateMemento> history)
        {
            if (IsDrawByRepetition(history))
            {
                return new GameResult(Player.None, EndGameTypes.ThreefoldRepetition);
            }

            return null;
        }

        private bool IsDrawByRepetition(IEnumerable<GameStateMemento> history)
        {
            if (history.Count() < 8) return false;

            var currentHash = history.Last().PositionHash;
            int occurrences = history.Count(m => m.PositionHash == currentHash);

            return occurrences >= 3;
        }
    }
}
