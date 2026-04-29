using ChessGame.Model;

namespace ChessGame.Services
{
    public interface IBoardFactory
    {
        Board CreateInitial();
    }
}
