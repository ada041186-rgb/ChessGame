using ChessApplication.Interfaces.Game;
using ChessLibrary.Game;

namespace ChessApplication.Services.Game
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
