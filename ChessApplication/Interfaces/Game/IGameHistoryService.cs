using ChessLibrary.Game;

namespace ChessApplication.Interfaces.Game
{
    public interface IGameHistoryService
    {
        void AddSnapshot(GameStateMemento memento);
        GameStateMemento Undo();
        IEnumerable<GameStateMemento> GetHistory();
        void Clear();
    }
}
