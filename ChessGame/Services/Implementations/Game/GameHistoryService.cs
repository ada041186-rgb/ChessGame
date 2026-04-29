using ChessGame.Model;
using ChessGame.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChessGame.Services.Implementations.Game
{
    public class GameHistoryService : IGameHistoryService
    {
        private readonly List<GameStateMemento> _history = new List<GameStateMemento>();

        public void AddSnapshot(GameStateMemento memento)
        {
            _history.Add(memento);
        }

        public GameStateMemento Undo()
        {
            if (_history.Count > 1)
            {
                _history.RemoveAt(_history.Count - 1);
                return _history.Last();
            }
            return null;
        }

        public IEnumerable<GameStateMemento> GetHistory()
        {
            return _history.AsReadOnly();
        }

        public void Clear()
        {
            _history.Clear();
        }
    }
}
