using ChessGame.Model.Moves;
using ChessGame.Services.Interfaces;
using System.Windows;


namespace ChessGame.Model.DTO.Handlers
{
    public class MoveHandler : IMessageHandler<DtoMove>
    {
        private readonly IGameState _gameState;
        private readonly IDtoMoveFactory _moveFactory;

        public MoveHandler(IDtoMoveFactory moveFactory, IGameState gameState)
        {
            _moveFactory = moveFactory;
            _gameState = gameState;
        }

        public Task HandleAsync(DtoMove message)
        {
            var move = _moveFactory.GetMoveFromDTO(message);

            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                try
                {
                    _gameState.MakeMove(move);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка при виконанні ходу: {ex.Message}", "MoveHandler Error");
                }
            });

            return Task.CompletedTask;
        }
    }
}
