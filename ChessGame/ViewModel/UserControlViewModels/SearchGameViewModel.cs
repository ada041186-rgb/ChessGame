using ChessGame.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ChessGame.ViewModel
{
    public class SearchGameViewModel : BaseViewModel
    {
        public ICommand JoinCommand { get; }
        public ICommand MenuCommand { get; }

        public SearchGameViewModel()
        {
            JoinCommand = new RelayCommand(SearchGame);
            MenuCommand = new RelayCommand(ReturnToMenu);
        }
        public void SearchGame(object parameter)
        {
            // Process of searching game
        }

        public void ReturnToMenu(object parameter)
        {
            MainViewModel.Instance.CurrentView = new MenuViewModel();
        }
    }
}
