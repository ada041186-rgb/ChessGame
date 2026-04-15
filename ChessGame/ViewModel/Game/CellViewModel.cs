using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace ChessGame.ViewModel
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

        public int Row { get; set; }
        public int Column { get; set; }
    }
}
