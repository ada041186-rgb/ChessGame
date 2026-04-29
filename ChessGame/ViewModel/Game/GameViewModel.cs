using ChessGame.Commands;
using ChessGame.Model;
using ChessGame.Model.Moves;
using ChessGame.Services.Implementations;
using ChessGame.Services.Interfaces;
using ChessGame.ViewModel.Game;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ChessGame.ViewModel
{
    public class GameViewModel : BaseViewModel, IDisposable
    {
        protected readonly Dictionary<Position, Move> moveCache = new Dictionary<Position, Move>();
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
        private readonly IGameService _gameService;
        private readonly INetworkService _networkService;
        private readonly INavigationService _navigationService;

        public ICommand CellClickCommand { get; }
        public GameViewModel(IGameService gameService, INetworkService networkService, INavigationService navigationService)
        {
            _gameService = gameService;
            _networkService = networkService;
            _navigationService = navigationService;

            InitializeBoard();

            CellClickCommand = new RelayCommand(OnCellClick);

            _gameService.BoardChanged += OnBoardUpdated;
            _gameService.MoveExecuted += OnMoveExecuted;
            _gameService.GameOver += OnGameOver;

            IsFlipped();
            OnBoardUpdated();
        }

        private void OnGameOver(GameResult result)
        {
            var app = (App)Application.Current;
            var serviceProvider = app.ServiceProvider;
            var endGameVM = serviceProvider.GetRequiredService<EndResultViewModel>();

            endGameVM.Initialize(result);

            _navigationService.NavigateTo(endGameVM);
        }

        private void OnMoveExecuted(Move move)
        {
            if (_gameService.IsCurrentPlayer())
            {
                _networkService.SendAsync(DtoType.Move, new DtoMove(move));
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
            moveCache.Clear();

            foreach (Move move in moves)
            {
                moveCache[move.ToPos] = move;
            }
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
            if (!moves.Any())
                return;

            SelectedPos = pos;
            CacheMoves(moves);
            HighlightsViewModel.ShowHighlights(moveCache.Keys);
        }
        private void OnToPositionSelected(Position pos)
        {
            SelectedPos = null;
            HighlightsViewModel.HideHighlights();

            if(moveCache.TryGetValue(pos, out Move move))
            {
                _gameService.TryMakeMove(move);
            }
        }

        public void Dispose()
        {
            _gameService.BoardChanged -= OnBoardUpdated;
            _gameService.MoveExecuted -= OnMoveExecuted;
            _gameService.GameOver -= OnGameOver;
        }
    }
}
