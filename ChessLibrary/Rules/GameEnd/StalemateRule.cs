using ChessLibrary.Board;
using ChessLibrary.Enums;
using ChessLibrary.Game;
using ChessLibrary.Rules.Validation;

namespace ChessLibrary.Rules.GameEnd
{
    public class StalemateRule : EndGameRuleHandler
    {
        private readonly IChessRulesEvaluator _rules;

        public StalemateRule(IChessRulesEvaluator rules)
        {
            _rules = rules;
        }

        public override GameResult? Check(IBoard board, Player nextPlayer, IEnumerable<GameStateMemento> history)
        {
            if (!_rules.IsInCheck(board, nextPlayer) &&
                !_rules.HasAnyLegalMoves(board, nextPlayer))
            {
                return new GameResult(Player.None, EndGameTypes.Stalemate);
            }

            return base.Check(board, nextPlayer, history);
        }
    }
}
