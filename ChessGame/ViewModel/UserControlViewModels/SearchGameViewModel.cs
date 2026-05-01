using ChessApplication.DTO;
using ChessApplication.Interfaces.Utils;
using ChessGame.Commands;
using ChessGame.Factories.ViewModelsFactories;
using ChessGame.Utils;
using ChessGame.ViewModel.Base;
using System.Windows;
using System.Windows.Input;

namespace ChessGame.ViewModel.UserControlViewModels
{
    public class SearchGameViewModel : BaseViewModel, IDisposable
    {
        private readonly INavigationService _navigation;
        private readonly IViewModelFactory<LobbyParams> _lobbyFactory;
        private readonly ILobbyService _lobbyService;

        private string _ipAddress = "127.0.0.1";

        public string IpAddress
        {
            get => _ipAddress;
            set
            {
                _ipAddress = value;
                NotifyPropertyChanged();
            }
        }

        public ICommand JoinCommand { get; }
        public ICommand MenuCommand { get; }

        public SearchGameViewModel( INavigationService navigation,
                                    IViewModelFactory<LobbyParams> lobbyFactory, 
                                    ILobbyService lobbyService)
        {
            _navigation = navigation;
            _lobbyFactory = lobbyFactory;
            _lobbyService = lobbyService;

            JoinCommand = new AsyncRelayCommand(JoinGame);
            MenuCommand = new RelayCommand(ReturnToMenu);

            _lobbyService.IsConnected += OnConnectionFinished;
        }

        private async Task JoinGame()
        {
            if (string.IsNullOrWhiteSpace(IpAddress)) return;

            var param = new LobbyParams(isHost: false, IpAddress);

            await _lobbyService.InitializeAsync(param);
        }

        private void OnConnectionFinished(bool success)
        {
            if (success)
            {
                var param = new LobbyParams(isHost: false, IpAddress);
                var lobbyVM = _lobbyFactory.CreateViewModelWithParams(param);

                _navigation.NavigateTo(lobbyVM);
            }
            else
            {
                MessageBox.Show("Не вдалося підключитися.");
            }
        }

        public void Dispose()
        {
            _lobbyService.IsConnected -= OnConnectionFinished;
        }

        private void ReturnToMenu(object parameter)
        {
            _navigation.NavigateTo<MenuViewModel>();

        }
    }
}