using ChessGame.Model;
using ChessGame.Model.Moves;
using ChessGame.Services.Interfaces;
using ChessGame.Services.Interfaces.Utils;

namespace ChessGame.Services
{
    public class GameService : IGameService
    {
        private IGameState _state;
        private readonly IGameStateFactory _stateFactory;
        private readonly IChessRulesService _rules;

        public Player ThisPlayer => _state.ThisPlayer;

        public event Action BoardChanged;
        public event Action PlayerChanged;
        public event Action<Move> MoveExecuted;

        public GameService(IGameStateFactory stateFactory, IChessRulesService rules)
        {
            _stateFactory = stateFactory;
            _rules = rules;
            _state = _stateFactory.Create(Player.None);
        }

        public void InitGame(Player player)
        {
            _state = _stateFactory.Create(player);
            RaiseEvents();
        }

        public bool IsCurrentPlayer()
        {
            return _state.ThisPlayer == _state.CurrentPlayer;
        }

        public Board GetBoard()
        {
            return _state.Board;
        }

        public bool TryMakeMove(Move move)
        {
            if (!_rules.IsMoveLegal(_state.Board, move))
            {
                return false;
            }

            move.Execute(_state.Board);
            MoveExecuted?.Invoke(move);

            _state.CurrentPlayer = _state.CurrentPlayer.Opponent();
            RaiseEvents();

            return true;
        }

        public IEnumerable<Move> GetLegalMoves(Position pos)
        {
            return _rules.GetLegalMoves(_state.Board, _state.CurrentPlayer, pos);
        }

        public Position GetKingInCheck()
        {
            return _rules.GetKingInCheck(_state.Board);
        }

        private void RaiseEvents()
        {
            BoardChanged?.Invoke();
            PlayerChanged?.Invoke();
        }
    }
}