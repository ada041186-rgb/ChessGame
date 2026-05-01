using ChessLibrary.Board;
using ChessLibrary.Enums;
using ChessLibrary.Factories;

namespace ChessLibrary.Game.Factories
{
    public class GameStateFactory : IGameStateFactory
    {
        private readonly IBoardFactory _boardFactory;

        public GameStateFactory(IBoardFactory boardFactory)
        {
            _boardFactory = boardFactory;
        }

        public IGameState Create(Player player)
        {
            IBoard board = _boardFactory.CreateInitial();

            var state = new GameState();
            state.Initialize(player, board);

            return state;
        }
    }
}
