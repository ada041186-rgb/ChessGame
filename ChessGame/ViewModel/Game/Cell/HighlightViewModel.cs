using ChessGame.ViewModel.Base;
using ChessLibrary.ValueObjects;
using System.Windows.Media;

namespace ChessGame.ViewModel.Game.Cell
{
    public class HighlightViewModel : BaseViewModel
    {
        private Brush _brush = Brushes.Transparent;
        public Brush Brush
        {
            get => _brush;
            set
            {
                _brush = value;
                NotifyPropertyChanged();
            }
        }
        public Position Position { get; set; }
    }
}
