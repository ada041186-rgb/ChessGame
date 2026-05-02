using ChessApplication.DTO;
using ChessApplication.Interfaces.Utils;
using ChessGame.Commands;
using ChessGame.Utils;
using ChessGame.ViewModel.Base;
using ChessGame.ViewModel.Game;
using ChessLibrary.Enums;
using System.Windows.Input;

namespace ChessGame.ViewModel.UserControlViewModels
{
    public class LobbyViewModel : BaseViewModel, IDisposable
    {
        private readonly ILobbyService _lobbyService;
        private readonly INavigationService _navigation;

        private string _headerText;
        private bool _isHost;
        private bool _isOtherPlayerConnected;

        public string HeaderText
        {
            get => _headerText;
            set { _headerText = value; NotifyPropertyChanged(); }
        }

        public bool IsHost
        {
            get => _isHost;
            set { _isHost = value; NotifyPropertyChanged(); }
        }

        public bool IsOtherPlayerConnected
        {
            get => _isOtherPlayerConnected;
            set
            {
                _isOtherPlayerConnected = value;
                NotifyPropertyChanged();
                UpdateState();
            }
        }

        public bool CanStartGame => IsHost && IsOtherPlayerConnected;

        public ICommand StartGameCommand { get; }
        public ICommand ExitToMenuCommand { get; }

        public LobbyViewModel(ILobbyService lobbyService, INavigationService navigation)
        {
            _lobbyService = lobbyService;
            _navigation = navigation;

            _lobbyService.IsConnected += OnConnected;
            _lobbyService.GameStarted += OnGameStarted;

            StartGameCommand = new AsyncRelayCommand(
                StartGameAsync,
                () => CanStartGame
            );

            ExitToMenuCommand = new AsyncRelayCommand(ExitToMenuAsync);
        }

        public async Task ConfigureAsync(LobbyParams lobbyParams)
        {
            IsHost = lobbyParams.IsHost;
            await _lobbyService.InitializeAsync(lobbyParams);
        }

        private void OnConnected(bool isConnected)
        {
            System.Diagnostics.Debug.WriteLine($"OnConnected called: {isConnected}");
            App.Current.Dispatcher.Invoke(() =>
            {
                System.Diagnostics.Debug.WriteLine($"Dispatcher: setting IsOtherPlayerConnected = {isConnected}");
                IsOtherPlayerConnected = isConnected;
                UpdateState();
            });
        }

        private void UpdateState()
        {
            HeaderText = IsHost
                ? (IsOtherPlayerConnected ? "Суперник приєднався" : "Очікування суперника")
                : "Очікування початку гри";

            NotifyPropertyChanged(nameof(CanStartGame));
            (StartGameCommand as AsyncRelayCommand)?.RaiseCanExecuteChanged();
        }

        private async Task StartGameAsync()
        {
            await _lobbyService.StartLanGameAsync(Player.White);
        }

        private void OnGameStarted(Player player)
        {
            _lobbyService.Reset();
            _navigation.NavigateTo<GameViewModel>();
        }

        private async Task ExitToMenuAsync()
        {
            await _lobbyService.DisconnectAsync();
            _lobbyService.Reset();

            _navigation.NavigateTo<MenuViewModel>();
        }

        public void Dispose()
        {
            _lobbyService.IsConnected -= OnConnected;
            _lobbyService.GameStarted -= OnGameStarted;
            _lobbyService.Reset();
        }
    }
}