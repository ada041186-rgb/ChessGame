using ChessGame.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Services.Interfaces.Utils
{
    public interface IGameStateFactory
    {
        IGameState Create(Player player);
    }
}
