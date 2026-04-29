using ChessGame.Model;
using ChessGame.Services.Interfaces;
using ChessGame.Services.Interfaces.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Services.Implementations.Utils
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
            var board = _boardFactory.CreateInitial();

            var state = new GameState();
            state.Initialize(player, board);

            return state;
        }
    }
}
