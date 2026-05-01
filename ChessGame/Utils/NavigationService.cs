using ChessGame.ViewModel.Base;
using Microsoft.Extensions.DependencyInjection;

namespace ChessGame.Utils
{
    public class NavigationService : INavigationService
    {
        private readonly IServiceProvider _provider;

        private BaseViewModel _currentView;
        public BaseViewModel CurrentView
        {
            get => _currentView;
            private set
            {
                if (_currentView is IDisposable disposableViewModel)
                {
                    disposableViewModel.Dispose();
                }

                _currentView = value;
                ViewChanged?.Invoke();
            }
        }

        public event Action ViewChanged;
        public NavigationService(IServiceProvider provider)
        {
            _provider = provider;
        }

        public void NavigateTo<T>() where T : BaseViewModel
        {
            var viewModel = _provider.GetRequiredService<T>();
            CurrentView = viewModel;
        }
        public void NavigateTo(BaseViewModel viewModel)
        {
            CurrentView = viewModel;
        }
    }
}
