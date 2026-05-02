using ChessLibrary.Board;
using ChessLibrary.Enums;
using ChessLibrary.Game;
using ChessLibrary.Rules.GameEnd;
using ChessLibrary.Rules.Validation;

namespace ChessLibrary.Rules
{
    public class EndGameEvaluator : IEndGameRulePipeline
    {
        private readonly EndGameRuleHandler _chain;

        public EndGameEvaluator(IChessRulesEvaluator rules)
        {
            var checkmate = new CheckmateRule(rules);
            var stalemate = new StalemateRule(rules);
            var repetition = new RepetitionRule();
            var insufficient = new InsufficientMaterialRule();

            checkmate
                .SetNext(stalemate)
                .SetNext(repetition)
                .SetNext(insufficient);

            _chain = checkmate;
        }

        public GameResult? Evaluate(IBoard board, Player player, IEnumerable<GameStateMemento> history)
        {
            return _chain.Check(board, player, history);
        }
    }
}
