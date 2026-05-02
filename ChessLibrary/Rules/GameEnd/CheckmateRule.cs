using ChessLibrary.Board;
using ChessLibrary.Enums;
using ChessLibrary.Extensions;
using ChessLibrary.Game;
using ChessLibrary.Rules.Validation;

namespace ChessLibrary.Rules.GameEnd
{
    public class CheckmateRule : EndGameRuleHandler
    {
        private readonly IChessRulesEvaluator _rules;

        public CheckmateRule(IChessRulesEvaluator rules)
        {
            _rules = rules;
        }

        public override GameResult? Check(IBoard board, Player nextPlayer, IEnumerable<GameStateMemento> history)
        {
            if (_rules.IsInCheck(board, nextPlayer) &&
                !_rules.HasAnyLegalMoves(board, nextPlayer))
            {
                return new GameResult(nextPlayer.Opponent(), EndGameTypes.Checkmate);
            }

            return base.Check(board, nextPlayer, history);
        }
    }
}
