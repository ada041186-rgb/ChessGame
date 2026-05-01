using ChessGame.Commands;
using ChessGame.Model.Data;
using ChessGame.Utils;
using ChessGame.ViewModel.Base;
using ChessLibrary.Enums;
using ChessLibrary.Moves;
using ChessLibrary.Moves.PawnMoves;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ChessGame.ViewModel.Game
{
    public class PawnPromotionViewModel : BaseViewModel
    {
        private readonly IEnumerable<Move> _availableMoves;
        public ObservableCollection<PromotionOption> PromotionOptions { get; set; }
        public event Action<Move> PromotionSelected;
        public ICommand PromoteCommand { get; }

        public PawnPromotionViewModel(IEnumerable<Move> moves, Player color)
        {
            _availableMoves = moves;
            PromoteCommand = new RelayCommand(OnPromote);
            PreparePromotion(color);
        }

        private void OnPromote(object parameter)
        {
            if (parameter is PieceType selectedType)
            {
                var finalMove = _availableMoves
                    .OfType<PawnPromotion>()
                    .FirstOrDefault(m => m.PromotionPieceType == selectedType);

                if (finalMove != null)
                {
                    PromotionSelected?.Invoke(finalMove);
                }
            }
        }

        private void PreparePromotion(Player color)
        {
            PromotionOptions = new ObservableCollection<PromotionOption>();

            var availableTypes = _availableMoves
                .OfType<PawnPromotion>()
                .Select(m => m.PromotionPieceType);

            foreach (var type in availableTypes)
            {
                PromotionOptions.Add(new PromotionOption
                {
                    Type = type,
                    Image = Images.GetImage(color, type)
                });
            }
        }


    }
}
