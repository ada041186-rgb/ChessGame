using ChessLibrary.Board;
using ChessLibrary.Enums;
using ChessLibrary.Game;

namespace ChessLibrary.Rules.GameEnd
{
    public class RepetitionRule : EndGameRuleHandler
    {
        public override GameResult? Check(
            IBoard board,
            Player nextPlayer,
            IEnumerable<GameStateMemento> history)
        {
            if (IsDrawByRepetition(history))
            {
                return new GameResult(Player.None, EndGameTypes.ThreefoldRepetition);
            }

            return base.Check(board, nextPlayer, history);
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
