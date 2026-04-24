using ChessGame.Model.DTO.Messages;
using ChessGame.Services.Interfaces;
using ChessGame.ViewModel;

namespace ChessGame.Model.DTO.Handlers
{
    public class StartGameHandler : IMessageHandler<DtoStartGame>
    {
        private readonly IGameState _gameState;
        private readonly INavigationService _navigationService;

        public StartGameHandler(IGameState gameState, INavigationService navigationService)
        {
            _gameState = gameState;
            _navigationService = navigationService;
        }

        public Task HandleAsync(DtoStartGame message)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                _gameState.Initialize(message.StartingSide);
                _navigationService.NavigateTo<GameViewModel>();
            });

            return Task.CompletedTask;
        }
    }
}
