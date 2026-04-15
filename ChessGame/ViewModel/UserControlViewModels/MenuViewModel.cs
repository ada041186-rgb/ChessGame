using ChessGame.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ChessGame.ViewModel
{
    public class MenuViewModel : BaseViewModel
    {
        public ICommand CreateGameCommand { get; }
        public ICommand SearchGameCommand { get; }
        public ICommand ExitCommand { get; }

        public MenuViewModel()
        {
            CreateGameCommand = new RelayCommand(CreateGame);
            SearchGameCommand = new RelayCommand(SearchGame);
            ExitCommand = new RelayCommand(Exit);
        }



        public void CreateGame(object parameter)
        {
            MainViewModel.Instance.CurrentView = new GameViewModel();
        }
        public void SearchGame(object parameter)
        {
            MainViewModel.Instance.CurrentView = new SearchGameViewModel();
        }
        public void Exit(object parameter)
        {
            Application.Current.Shutdown();
        }
    }
}
