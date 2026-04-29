using ChessGame.Model;
using ChessGame.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Services.Implementations.Game
{
    public class EndGameRulePipeline : IEndGameRulePipeline
    {
        private readonly IEnumerable<IEndGameRule> _rules;

        public EndGameRulePipeline(IEnumerable<IEndGameRule> rules)
        {
            _rules = rules;
        }

        public GameResult Evaluate(Board board, Player nextPlayer, IEnumerable<GameStateMemento> history)
        {
            foreach (var rule in _rules)
            {
                var result = rule.Check(board, nextPlayer, history);

                if (result != null)
                    return result;
            }

            return null;
        }
    }
}
