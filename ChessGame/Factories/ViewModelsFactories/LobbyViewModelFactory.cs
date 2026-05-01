using ChessApplication.DTO;
using ChessGame.ViewModel.Base;
using ChessGame.ViewModel.UserControlViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace ChessGame.Factories.ViewModelsFactories
{
    public class LobbyViewModelFactory : IViewModelFactory<LobbyParams>
    {
        private readonly IServiceProvider _serviceProvider;
        public LobbyViewModelFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public BaseViewModel CreateViewModelWithParams(LobbyParams param)
        {
            var lobbyVM = _serviceProvider.GetRequiredService<LobbyViewModel>();


            lobbyVM.ConfigureAsync(param);

            return lobbyVM;
        }
    }
}
