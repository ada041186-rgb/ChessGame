using ChessGame.Commands;
using ChessGame.Services.Implementations;
using ChessGame.Services.Interfaces;
using System.Windows.Input;

namespace ChessGame.ViewModel
{
    public class LobbyViewModel : BaseViewModel, IDisposable
    {
        private readonly ILobbyService _lobbyService;
        private readonly INavigationService _navigation;

        private string _headerText;
        public string HeaderText
        {
            get { return _headerText; }
            set { _headerText = value; NotifyPropertyChanged(); }
        }
        private bool _isHost;
        public bool IsHost
        {
            get { return _isHost; }
            set { _isHost = value; NotifyPropertyChanged(); }
        }
        private bool _isOtherPlayerConnected;
        public bool IsOtherPlayerConnected
        {
            get { return _isOtherPlayerConnected; }
            set { _isOtherPlayerConnected = value; NotifyPropertyChanged(); }
        }

        public bool CanStartGame => IsHost && IsOtherPlayerConnected;
        public ICommand StartGameCommand { get; }

        public LobbyViewModel(ILobbyService lobbyService, INavigationService navigation)
        {
            _lobbyService = lobbyService;
            _navigation = navigation;

            _lobbyService.IsConnected += OnConnected;

            StartGameCommand = new AsyncRelayCommand(
                StartGameAsync,
                () => CanStartGame
            );
        }

        private void OnConnected(bool isConnected)
        {
            IsOtherPlayerConnected = isConnected;
            UpdateHeader();

            NotifyPropertyChanged(nameof(CanStartGame));

            (StartGameCommand as AsyncRelayCommand)?.RaiseCanExecuteChanged();
        }
        private void UpdateHeader()
        {
            HeaderText = IsHost
                ? (IsOtherPlayerConnected ? "Суперник приєднався" : "Очікування суперника")
                : "Очікування початку гри";
        }
        public async Task ConfigureAsync(bool isHost, string ip = null)
        {
            IsHost = isHost;

            await _lobbyService.InitializeAsync(isHost, ip);
        }

        private async Task StartGameAsync()
        {
            await _lobbyService.StartGameAsync();

            _navigation.NavigateTo<GameViewModel>();
        }


        public void Dispose()
        {
            _lobbyService.IsConnected -= OnConnected;
        }
    }
}