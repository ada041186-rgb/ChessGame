using ChessGame.Services.Interfaces;
using ChessGame.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Services
{
    public class NavigationService : BaseViewModel, INavigationService
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
                NotifyPropertyChanged();
            }
        }

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
