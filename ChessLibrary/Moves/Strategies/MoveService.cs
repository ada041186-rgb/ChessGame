using ChessLibrary.Board;
using ChessLibrary.Pieces;
using ChessLibrary.ValueObjects;

namespace ChessLibrary.Moves.Strategies
{
    public class MoveService : IMoveService
    {
        private readonly IEnumerable<IMoveStrategy> _strategies;

        public MoveService(IEnumerable<IMoveStrategy> strategies)
        {
            _strategies = strategies;
        }

        public IEnumerable<Move> GetMoves(IPiece piece, Position from, IBoard board)
        {
            if (piece == null)
                return Enumerable.Empty<Move>();

            return _strategies
                .Where(s => s.CanHandle(piece))
                .SelectMany(s => s.GetMoves(from, board, piece));
        }
    }
}
