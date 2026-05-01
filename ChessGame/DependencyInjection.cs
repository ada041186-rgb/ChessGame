// FILE: ChessGame/DependencyInjection.cs  (новий файл у WPF-проєкті)
// Цей файл замінює розкидані виклики AddAppServices / AddFactories / тощо в App.xaml.cs
using ChessApplication.DTO;
using ChessGame.Factories.ViewModelsFactories;
using ChessGame.Utils;
using ChessGame.ViewModel;
using ChessGame.ViewModel.Game;
using ChessGame.ViewModel.UserControlViewModels;
using ChessLibrary.Game;
using Microsoft.Extensions.DependencyInjection;

namespace ChessGame
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Registers all presentation-layer services: navigation, VM factories, ViewModels.
        /// </summary>
        public static IServiceCollection AddChessPresentation(this IServiceCollection services)
        {
            services.AddNavigation();
            services.AddViewModelFactories();
            services.AddViewModels();

            return services;
        }

        private static IServiceCollection AddNavigation(this IServiceCollection services)
        {
            services.AddSingleton<INavigationService, NavigationService>();

            return services;
        }

        private static IServiceCollection AddViewModelFactories(this IServiceCollection services)
        {
            services.AddSingleton<IViewModelFactory<GameResult>, EndResultViewModelFactory>();
            services.AddSingleton<IViewModelFactory<LobbyParams>, LobbyViewModelFactory>();

            return services;
        }

        private static IServiceCollection AddViewModels(this IServiceCollection services)
        {
            services.AddSingleton<MainViewModel>();

            services.AddTransient<MenuViewModel>();
            services.AddTransient<SearchGameViewModel>();
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<LobbyViewModel>();
            services.AddTransient<GameViewModel>();
            services.AddTransient<EndResultViewModel>();

            return services;
        }
    }
}