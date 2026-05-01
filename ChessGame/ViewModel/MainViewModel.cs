using ChessApplication.Interfaces.Game;
using ChessGame.Utils;
using ChessGame.ViewModel.Base;
using ChessLibrary.Enums;

namespace ChessGame.ViewModel
{
    public class MainViewModel : BaseViewModel, IDisposable
    {
        private readonly INavigationService _navigation;
        private readonly IGameService _gameService;

        public BaseViewModel CurrentView => _navigation.CurrentView;
        public Player CurrentPlayerSide => _gameService.ThisPlayer;

        public MainViewModel(INavigationService navigation, IGameService gameService)
        {
            _gameService = gameService;
            _navigation = navigation;

            _navigation.ViewChanged += OnViewChanged;
            _gameService.PlayerChanged += OnPlayerChanged;
        }
        private void OnPlayerChanged()
        {
            NotifyPropertyChanged(nameof(CurrentPlayerSide));
        }
        private void OnViewChanged()
        {
            NotifyPropertyChanged(nameof(CurrentView));
        }
        public void Dispose()
        {
            _navigation.ViewChanged -= OnViewChanged;
            _gameService.PlayerChanged -= OnPlayerChanged;
        }
    }
}
