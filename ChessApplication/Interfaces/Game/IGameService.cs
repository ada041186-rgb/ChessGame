using ChessLibrary.Board;
using ChessLibrary.Enums;
using ChessLibrary.Game;
using ChessLibrary.Moves;
using ChessLibrary.ValueObjects;

namespace ChessApplication.Interfaces.Game
{
    public interface IGameService
    {
        Player ThisPlayer { get; }
        Player CurrentPlayer { get; }

        event Action BoardChanged;
        event Action PlayerChanged;
        event Action<Move> MoveExecuted;
        event Action<GameResult> GameOver;

        void InitGame(Player player);
        bool TryMakeMove(Move move);
        IEnumerable<Move> GetLegalMoves(Position pos);
        bool IsCurrentPlayer();
        IBoard GetBoard();
        Position GetKingInCheck();
    }
}