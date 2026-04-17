using ChessGame.Commands;
using ChessGame.Model;
using ChessGame.ViewModel.Game;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ChessGame.ViewModel
{
    public class GameViewModel : BaseViewModel
    {
        public ObservableCollection<CellViewModel> BoardCells { get; set; } = new ObservableCollection<CellViewModel>();
        public ObservableCollection<HighlightViewModel> HighlightCells { get; set; } = new ObservableCollection<HighlightViewModel>();

        private readonly Dictionary<Position, Move> moveCache = new Dictionary<Position, Move>();
        private Position SelectedPos { get; set; }

        private GameState GameState { get; }
        public ICommand CellClickCommand { get; }
        public GameViewModel()
        {
            InitializeBoard();

            GameState = new GameState(Player.White, Board.Initial());
            CellClickCommand = new RelayCommand(OnCellClick);
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
                        Position = new Position(r, c),
                        ImagePath = null
                    });

                    HighlightCells.Add(new HighlightViewModel
                    {
                        Position = new Position(r, c),
                        Brush = Brushes.Transparent
                    });
                }
            }
        }

        private void CacheMoves(IEnumerable<Move> moves)
        {
            moveCache.Clear();

            foreach (Move move in moves)
            {
                moveCache[move.ToPos] = move;
            }
        }

        private void ShowHighlights()
        {
            Color color = Color.FromArgb(150, 125, 255, 125);

            foreach (Position to in moveCache.Keys)
            {
                HighlightCells.First(item => item.Position == to).Brush = new SolidColorBrush(color);
            }
        }

        private void HideHighlights()
        {
            foreach (Position to in moveCache.Keys)
            {
                HighlightCells.First(item => item.Position == to).Brush = Brushes.Transparent;
            }
        }
        private void OnCellClick(object obj)
        {
            if (obj is not Position pos)
            {
                return;
            }

            if (SelectedPos == null)
            {
                OnFromPositionSelected(pos);
            }
            else
            {
                OnToPositionSelected(pos);

            }
        }

        private void OnFromPositionSelected(Position pos)
        {
            IEnumerable<Move> moves = GameState.LegalMovesForPiece(pos);

            if (moves.Any())
            {
                SelectedPos = pos;
                CacheMoves(moves);
                ShowHighlights();
            }
        }
        private void OnToPositionSelected(Position pos)
        {
            SelectedPos = null;
            HideHighlights();

            if(moveCache.TryGetValue(pos, out Move move))
            {
                HandleMove(move);
            }
        }

        private void HandleMove(Move move)
        {
            GameState.MakeMove(move);
            DrawBoard(GameState.Board);
        }

        private void DrawBoard(Board board)
        {
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    Piece piece = board[r, c];
                    BoardCells.First(item => item.Position.Row == r && item.Position.Column == c).ImagePath = Images.GetImage(piece);
                }
            }
        }
    }
}
