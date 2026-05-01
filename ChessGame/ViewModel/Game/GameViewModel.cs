using ChessApplication.Interfaces.Game;
using ChessApplication.Interfaces.Network;
using ChessGame.Commands;
using ChessGame.Factories.ViewModelsFactories;
using ChessGame.Utils;
using ChessGame.ViewModel.Base;
using ChessGame.ViewModel.Game.Cells;
using ChessGame.ViewModel.UserControlViewModels;
using ChessLibrary.Enums;
using ChessLibrary.Game;
using ChessLibrary.Moves;
using ChessLibrary.Moves.PawnMoves;
using ChessLibrary.ValueObjects;
using System.Windows;
using System.Windows.Input;

namespace ChessGame.ViewModel.Game
{
    public class GameViewModel : BaseViewModel, IDisposable
    {
        private readonly IGameService _gameService;
        private readonly INetworkService _networkService;
        private readonly INavigationService _navigationService;
        private readonly IDtoMoveFactory _dtoMoveFactory;
        private readonly IViewModelFactory<GameResult> _gameResultFactory;


        private readonly MoveCache _moveCache = new();
        public CellsViewModel CellsViewModel { get; set; }
        public HighlightsViewModel HighlightsViewModel { get; set; }
        private Position SelectedPos { get; set; }
        private double _boardRotation;
        public double BoardRotation
        {
            get => _boardRotation;
            set
            {
                _boardRotation = value;
                NotifyPropertyChanged();
            }
        }
        private PawnPromotionViewModel _promotionViewModel;
        public PawnPromotionViewModel PromotionViewModel
        {
            get => _promotionViewModel;
            set { _promotionViewModel = value; NotifyPropertyChanged(); }
        }

        private string _turnStatus;
        public string TurnStatus
        {
            get => _turnStatus;
            set { _turnStatus = value; NotifyPropertyChanged(); }
        }

        private string _whiteTimeText = "00:00";
        public string WhiteTimeText
        {
            get => _whiteTimeText;
            set
            {
                _whiteTimeText = value;
                NotifyPropertyChanged();
            }
        }

        private string _blackTimeText = "00:00";
        public string BlackTimeText
        {
            get => _blackTimeText;
            set
            {
                _blackTimeText = value;
                NotifyPropertyChanged();
            }
        }
        public ICommand LeaveGameCommand { get; }
        public ICommand CellClickCommand { get; }
        public GameViewModel(IGameService gameService, INetworkService networkService,
            INavigationService navigationService, IDtoMoveFactory dtoMoveFactory, IViewModelFactory<GameResult> gameResultFactory)
        {
            _gameService = gameService;
            _networkService = networkService;
            _navigationService = navigationService;
            _dtoMoveFactory = dtoMoveFactory;
            _gameResultFactory = gameResultFactory;

            InitializeBoard();

            CellClickCommand = new RelayCommand(OnCellClick);
            LeaveGameCommand = new RelayCommand(OnLeaveGame);

            _gameService.BoardChanged += OnBoardUpdated;
            _gameService.MoveExecuted += OnMoveExecuted;
            _gameService.GameOver += OnGameOver;

            IsFlipped();
            OnBoardUpdated();
        }

        private void OnGameOver(GameResult result)
        {
            var endGameVM = _gameResultFactory.CreateViewModelWithParams(result);

            _navigationService.NavigateTo(endGameVM);
        }

        private void OnMoveExecuted(Move move)
        {
            if (_gameService.IsCurrentPlayer())
            {
                var dtoMessage = _dtoMoveFactory.GetMoveToDTO(move);

                _networkService.SendAsync(dtoMessage.MessageType, dtoMessage);
            }
        }
        private void OnBoardUpdated()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                SelectedPos = null;
                HighlightsViewModel.HideHighlights();

                var board = _gameService.GetBoard();

                CellsViewModel.DrawBoard(board);

                HighlightsViewModel.ShowCheck(
                    _gameService.GetKingInCheck()
                );
                UpdateTurnStatus();
            });
        }

        private void IsFlipped()
        {
            BoardRotation = _gameService.ThisPlayer == Player.Black ? 180 : 0;
        }

        private void InitializeBoard()
        {
            CellsViewModel = new CellsViewModel();
            HighlightsViewModel = new HighlightsViewModel();

            CellsViewModel.InitializeCells();
            HighlightsViewModel.InitializeHighlights();
        }
        private void CacheMoves(IEnumerable<Move> moves)
        {
            _moveCache.CacheMoves(moves);
        }
        private void OnCellClick(object obj)
        {
            if (obj is not Position pos)
                return;

            if (!_gameService.IsCurrentPlayer())
                return;

            if (SelectedPos == null)
            {
                OnFromPositionSelected(pos);
            }
            else
            {
                OnToPositionSelected(pos);
            }

            HighlightsViewModel.ShowCheck(
                _gameService.GetKingInCheck()
            );
        }

        private void OnFromPositionSelected(Position pos)
        {
            var moves = _gameService.GetLegalMoves(pos);
            if (!moves.Any()) return;

            SelectedPos = pos;
            CacheMoves(moves);
            HighlightsViewModel.ShowHighlights(_moveCache.GetDestinations());
        }
        private void OnToPositionSelected(Position pos)
        {
            SelectedPos = null;
            HighlightsViewModel.HideHighlights();

            if (_moveCache.HasMovesForTarget(pos))
            {
                var movesToPos = _moveCache.GetMovesForTarget(pos);
                if (!movesToPos.Any()) return;

                if (movesToPos.Any(m => m is PawnPromotion))
                {
                    var promotionVM = new PawnPromotionViewModel(movesToPos, _gameService.ThisPlayer);
                    promotionVM.PromotionSelected += (finalMove) =>
                    {
                        _gameService.TryMakeMove(finalMove);
                        PromotionViewModel = null;
                    };
                    PromotionViewModel = promotionVM;
                }
                else
                {
                    _gameService.TryMakeMove(movesToPos.First());
                }
            }
        }
        private void OnLeaveGame(object obj)
        {
            //_networkService.SendAsync("Resign", new { Player = _gameService.ThisPlayer });

            _navigationService.NavigateTo<MenuViewModel>();
        }

        private void UpdateTurnStatus()
        {
            var currentPlayer = _gameService.CurrentPlayer;
            TurnStatus = currentPlayer == Player.White ? "Хід Білих" : "Хід Чорних";
        }
        public void Dispose()
        {
            _gameService.BoardChanged -= OnBoardUpdated;
            _gameService.MoveExecuted -= OnMoveExecuted;
            _gameService.GameOver -= OnGameOver;
        }
    }
}
