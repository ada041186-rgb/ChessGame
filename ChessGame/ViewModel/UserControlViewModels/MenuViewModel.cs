using ChessApplication.DTO;
using ChessGame.Commands;
using ChessGame.Factories.ViewModelsFactories;
using ChessGame.Utils;
using ChessGame.ViewModel.Base;
using System.Windows;
using System.Windows.Input;

namespace ChessGame.ViewModel.UserControlViewModels
{
    public class MenuViewModel : BaseViewModel
    {
        private readonly INavigationService _navigation;

        private readonly IViewModelFactory<LobbyParams> _lobbyFactory;

        public ICommand CreateGameCommand { get; }
        public ICommand SearchGameCommand { get; }
        public ICommand SettingsCommand { get; }
        public ICommand ExitCommand { get; }

        public MenuViewModel(
            INavigationService navigation,
            IViewModelFactory<LobbyParams> lobbyFactory)
        {
            _navigation = navigation;
            _lobbyFactory = lobbyFactory;

            CreateGameCommand = new RelayCommand(CreateGameAsync);
            SearchGameCommand = new RelayCommand(SearchGame);
            SettingsCommand = new RelayCommand(Settings);
            ExitCommand = new RelayCommand(Exit);
        }

        private void CreateGameAsync(object obj)
        {
            var param = new LobbyParams(isHost: true);

            var lobbyVM = _lobbyFactory.CreateViewModelWithParams(param);

            _navigation.NavigateTo(lobbyVM);
        }

        public void SearchGame(object obj)
        {
            _navigation.NavigateTo<SearchGameViewModel>();
        }

        public void Settings(object obj)
        {
            _navigation.NavigateTo<SettingsViewModel>();
        }

        public void Exit(object obj)
        {
            Application.Current.Shutdown();
        }
    }
}