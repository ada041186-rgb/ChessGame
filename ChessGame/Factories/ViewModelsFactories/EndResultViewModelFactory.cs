using ChessGame.ViewModel.Base;
using ChessGame.ViewModel.Game;
using ChessLibrary.Game;
using Microsoft.Extensions.DependencyInjection;

namespace ChessGame.Factories.ViewModelsFactories
{
    public class EndResultViewModelFactory : IViewModelFactory<GameResult>
    {
        private readonly IServiceProvider _serviceProvider;
        public EndResultViewModelFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public BaseViewModel CreateViewModelWithParams(GameResult param)
        {
            var vm = _serviceProvider.GetRequiredService<EndResultViewModel>();

            vm.Initialize(param);

            return vm;
        }
    }
}
