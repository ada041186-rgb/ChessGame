using ChessGame.Model;

namespace ChessGame.Services.Interfaces
{
    public interface IGameService
    {
        Player ThisPlayer { get; }

        event Action BoardChanged;
        event Action PlayerChanged;
        event Action<Move> MoveExecuted;
        event Action<GameResult> GameOver;

        void InitGame(Player player);
        bool TryMakeMove(Move move);
        IEnumerable<Move> GetLegalMoves(Position pos);
        bool IsCurrentPlayer();
        Board GetBoard();
        Position GetKingInCheck();
    }
}