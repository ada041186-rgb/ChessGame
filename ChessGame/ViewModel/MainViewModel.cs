using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private static MainViewModel _instance;
        public static MainViewModel Instance => _instance ??= new MainViewModel();

        private BaseViewModel _currentView = new MenuViewModel();
        public BaseViewModel CurrentView
        {
            get { return _currentView; }
            set
            {
                if (_currentView != value)
                {
                    _currentView = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private MainViewModel()
        {
        }
    }
}
