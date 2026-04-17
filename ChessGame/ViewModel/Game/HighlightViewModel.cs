using ChessGame.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ChessGame.ViewModel.Game
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
