using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Model
{
    public interface IEndGameRule
    {
        GameResult Check(Board board, Player nextPlayer, IEnumerable<GameStateMemento> history);
    }
}
