using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Model
{
    public class GameState
    {
        public Player CurrentPlayer { get; private set; }
        public Board Board { get; }

        public GameState(Player currentPlayer, Board board) 
        {
            Board = board;
            CurrentPlayer = currentPlayer;
        }

    }
}
