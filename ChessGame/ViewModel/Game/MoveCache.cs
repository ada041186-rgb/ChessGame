using ChessLibrary.Moves;
using ChessLibrary.ValueObjects;

namespace ChessGame.ViewModel.Game
{
    public class MoveCache
    {
        private readonly Dictionary<Position, List<Move>> _cache = new();
        public void Clear() => _cache.Clear();
        public IEnumerable<Position> GetDestinations() => _cache.Keys;
        public void CacheMoves(IEnumerable<Move> moves)
        {
            Clear();
            foreach (var move in moves)
            {
                if (!_cache.ContainsKey(move.ToPos))
                    _cache[move.ToPos] = new List<Move>();

                _cache[move.ToPos].Add(move);
            }
        }

        public IReadOnlyList<Move> GetMovesForTarget(Position target)
        {
            return _cache.TryGetValue(target, out var moves) ? moves.AsReadOnly() : new List<Move>().AsReadOnly();
        }

        public bool HasMovesForTarget(Position target) => _cache.ContainsKey(target);
    }
}