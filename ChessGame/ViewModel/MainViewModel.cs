using ChessGame.Model;
using ChessGame.Services;
using ChessGame.Services.Interfaces;
using System.ComponentModel;

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

            _navigation.PropertyChanged += OnNavigationPropertyChanged;
            _gameService.PlayerChanged += OnPlayerChanged;
        }
        private void OnPlayerChanged()
        {
            NotifyPropertyChanged(nameof(CurrentPlayerSide));
        }
        private void OnNavigationPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(INavigationService.CurrentView))
            {
                NotifyPropertyChanged(nameof(CurrentView));
            }
        }
        public void Dispose()
        {
            _navigation.PropertyChanged -= OnNavigationPropertyChanged;
            _gameService.PlayerChanged -= OnPlayerChanged;
        }
    }
}
