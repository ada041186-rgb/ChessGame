using ChessGame.Utils;
using ChessGame.ViewModel.Base;
using ChessGame.ViewModel.Game.Cell;
using ChessLibrary.Board;
using ChessLibrary.Pieces;
using ChessLibrary.ValueObjects;
using System.Collections.ObjectModel;

namespace ChessGame.ViewModel.Game.Cells
{
    public class CellsViewModel : BaseViewModel
    {
        public ObservableCollection<CellViewModel> BoardCells { get; set; } = new ObservableCollection<CellViewModel>();
        public CellsViewModel()
        {

        }
        public void InitializeCells()
        {
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    BoardCells.Add(new CellViewModel
                    {
                        Position = new Position(r, c),
                        ImagePath = null
                    });
                }
            }
        }

        public void DrawBoard(IBoard board)
        {
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    IPiece piece = board[r, c];
                    int index = r * 8 + c;
                    BoardCells[index].ImagePath = Images.GetImage(piece);
                }
            }
        }
    }
}
