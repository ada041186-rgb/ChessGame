using ChessGame.ViewModel.Base;
using ChessLibrary.ValueObjects;
using System.Windows.Media;

namespace ChessGame.ViewModel.Game.Cell
{
    public class CellViewModel : BaseViewModel
    {
        private ImageSource _imagePath;
        public ImageSource ImagePath
        {
            get => _imagePath;
            set
            {
                _imagePath = value;
                NotifyPropertyChanged();
            }
        }
        public Position Position { get; set; }
    }
}
