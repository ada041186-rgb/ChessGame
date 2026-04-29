using ChessGame.Model;

namespace ChessGame.Services
{
    public class GameState : IGameState
    {
        public Player ThisPlayer { get; private set; }
        public Player CurrentPlayer { get; set; }
        public Board Board { get; private set; }

        public void Initialize(Player player, Board board)
        {
            ThisPlayer = player;
            CurrentPlayer = Player.White;
            Board = board;
        }

        public bool IsInitialized => Board != null;
    }
}