using ChessLibrary.Enums;

namespace ChessLibrary.Game.Factories
{
    public interface IGameStateFactory
    {
        IGameState Create(Player player);
    }
}
