using ChessGame.ViewModel.Base;
using ChessGame.ViewModel.Game.Cell;
using ChessLibrary.ValueObjects;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace ChessGame.ViewModel.Game.Cells
{
    public class HighlightsViewModel : BaseViewModel
    {
        public ObservableCollection<HighlightViewModel> HighlightCells { get; } = new ObservableCollection<HighlightViewModel>();
        private List<Position> _currentlyHighlighted = new List<Position>();

        public HighlightsViewModel() { }

        public void InitializeHighlights()
        {
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    HighlightCells.Add(new HighlightViewModel
                    {
                        Position = new Position(r, c),
                        Brush = Brushes.Transparent
                    });
                }
            }
        }

        public void ShowHighlights(IEnumerable<Position> positionsToHighlight)
        {
            HideHighlights();

            Color color = Color.FromArgb(150, 125, 255, 125);
            Brush highlightBrush = new SolidColorBrush(color);

            foreach (Position pos in positionsToHighlight)
            {
                int index = pos.Row * 8 + pos.Column;
                HighlightCells[index].Brush = highlightBrush;
                _currentlyHighlighted.Add(pos);
            }
        }
        public void ShowCheck(Position kingPos)
        {
            if (kingPos == null)
                return;

            Color color = Color.FromArgb(180, 255, 80, 80);
            Brush checkBrush = new SolidColorBrush(color);

            int index = kingPos.Row * 8 + kingPos.Column;
            HighlightCells[index].Brush = checkBrush;

            _currentlyHighlighted.Add(kingPos);
        }
        public void HideHighlights()
        {
            foreach (Position pos in _currentlyHighlighted)
            {
                int index = pos.Row * 8 + pos.Column;
                HighlightCells[index].Brush = Brushes.Transparent;
            }
            _currentlyHighlighted.Clear();
        }
    }
}