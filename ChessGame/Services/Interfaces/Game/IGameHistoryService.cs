using ChessGame.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Services.Interfaces
{
    public interface IGameHistoryService
    {
        void AddSnapshot(GameStateMemento memento);
        GameStateMemento Undo();
        IEnumerable<GameStateMemento> GetHistory();
        void Clear();
    }
}
