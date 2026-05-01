using ChessApplication.Interfaces.Network;
using ChessGame.Commands;
using ChessGame.Utils;
using ChessGame.ViewModel.Base;
using ChessGame.ViewModel.UserControlViewModels;
using ChessLibrary.Enums;
using ChessLibrary.Game;
using System.Windows.Input;

namespace ChessGame.ViewModel.Game
{
    public class EndResultViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly INetworkService _networkService;

        private string _titleText;
        public string TitleText
        {
            get => _titleText;
            set
            {
                _titleText = value;
                NotifyPropertyChanged();
            }
        }

        private string _reasonText;
        public string ReasonText
        {
            get => _reasonText;
            set
            {
                _reasonText = value;
                NotifyPropertyChanged();
            }
        }

        public ICommand BackToMenuCommand { get; }

        public EndResultViewModel(INavigationService navigationService, INetworkService networkService)
        {
            _navigationService = navigationService;
            _networkService = networkService;
            BackToMenuCommand = new AsyncRelayCommand(BackToMenu);
        }

        public void Initialize(GameResult result)
        {
            if (result.Winner != Player.None)
            {
                TitleText = $"Перемога: {result.Winner.ToString()}!";
            }
            else
            {
                TitleText = "Нічия!";
            }

            ReasonText = $"Причина: {result.Type.ToString()}";
        }

        private async Task BackToMenu()
        {
            await _networkService.DisconnectAsync();

            _navigationService.NavigateTo<MenuViewModel>();
        }
    }
}
