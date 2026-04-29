using ChessGame.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Services.Interfaces
{
    public interface IEndGameRulePipeline
    {
        public GameResult Evaluate(Board board, Player nextPlayer, IEnumerable<GameStateMemento> history);
    }
}
