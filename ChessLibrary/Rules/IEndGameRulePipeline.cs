using ChessLibrary.Board;
using ChessLibrary.Enums;
using ChessLibrary.Game;

namespace ChessLibrary.Rules
{
    public interface IEndGameRulePipeline
    {
        public GameResult? Evaluate(IBoard board, Player nextPlayer, IEnumerable<GameStateMemento> history);
    }
}
