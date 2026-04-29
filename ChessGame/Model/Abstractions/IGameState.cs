using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Model
{
    public interface IGameState
    {
        Player ThisPlayer { get; }
        Player CurrentPlayer { get; set; }
        Board Board { get; }

        void Initialize(Player thisPlayer, Board board);
        GameStateMemento SaveState();
        void RestoreState(GameStateMemento memento);
    }
}
