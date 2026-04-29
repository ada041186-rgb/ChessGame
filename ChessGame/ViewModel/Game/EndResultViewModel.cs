using ChessGame.Commands;
using ChessGame.Model;
using ChessGame.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ChessGame.ViewModel
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
