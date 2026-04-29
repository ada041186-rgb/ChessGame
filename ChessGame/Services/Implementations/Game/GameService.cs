using ChessGame.Model;
using ChessGame.Model.Enums;
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
        private readonly IGameHistoryService _history;
        private readonly IEndGameRulePipeline _endGamePipeline;

        public Player ThisPlayer => _state.ThisPlayer;

        public event Action BoardChanged;
        public event Action PlayerChanged;
        public event Action<Move> MoveExecuted;
        public event Action<GameResult> GameOver;

        public GameService(
            IGameStateFactory stateFactory, IChessRulesService rules,
            IGameHistoryService history, IEndGameRulePipeline endGameRulePipeline)
        {
            _stateFactory = stateFactory;
            _rules = rules;
            _state = _stateFactory.Create(Player.None);
            _history = history;
            _endGamePipeline = endGameRulePipeline;
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

            var nextPlayer = _state.CurrentPlayer.Opponent();
            _state.CurrentPlayer = nextPlayer;

            CheckGameEndConditions(nextPlayer);

            var snapshot = _state.SaveState();
            _history.AddSnapshot(snapshot);

            RaiseEvents();
            return true;
        }

        private void CheckGameEndConditions(Player nextPlayer)
        {
            var result = _endGamePipeline.Evaluate(
                _state.Board,
                nextPlayer,
                _history.GetHistory()
            );

            if (result != null)
            {
                _history.Clear();
                GameOver?.Invoke(result);
            }
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