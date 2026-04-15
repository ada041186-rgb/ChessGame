using ChessGame.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ChessGame.ViewModel
{
    public class GameViewModel : BaseViewModel
    {
        public ObservableCollection<CellViewModel> BoardCells { get; set; } = new ObservableCollection<CellViewModel>();
        private GameState GameState { get; }

        public GameViewModel()
        {
            InitializeBoard();

            GameState = new GameState(Player.White, Board.Initial());
            DrawBoard(GameState.Board);
        }

        private void InitializeBoard()
        {
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    BoardCells.Add(new CellViewModel
                    {
                        Row = r,
                        Column = c,
                        ImagePath = null
                    });
                }
            }
        }


        private void DrawBoard(Board board)
        {
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    Piece piece = board[r, c];
                    BoardCells.First(item => item.Row == r && item.Column == c).ImagePath = Images.GetImage(piece);
                }
            }
        }
    }
}
