using ChessLibrary.Board;
using ChessLibrary.Enums;
using ChessLibrary.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary.Rules.GameEnd
{
    public abstract class EndGameRuleHandler : IEndGameRule
    {
        protected EndGameRuleHandler? Next;

        public EndGameRuleHandler SetNext(EndGameRuleHandler next)
        {
            Next = next;
            return next;
        }

        public virtual GameResult? Check(
            IBoard board,
            Player nextPlayer,
            IEnumerable<GameStateMemento> history)
        {
            return Next?.Check(board, nextPlayer, history);
        }
    }
}
