using ChessGame.Model.Moves;
using ChessGame.Services.Interfaces;
using System.Windows;


namespace ChessGame.Model.DTO.Handlers
{
    public class MoveHandler : IMessageHandler<DtoMove>
    {
        private readonly IGameService _gameService;
        private readonly IDtoMoveFactory _moveFactory;

        public MoveHandler(IDtoMoveFactory moveFactory, IGameService gameService)
        {
            _moveFactory = moveFactory;
            _gameService = gameService;
        }

        public Task HandleAsync(DtoMove message)
        {
            var move = _moveFactory.GetMoveFromDTO(message);

            Application.Current.Dispatcher.Invoke(() =>
            {
                try
                {
                    _gameService.TryMakeMove(move);
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
