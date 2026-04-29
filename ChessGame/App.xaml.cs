using ChessGame.Model;
using ChessGame.Model.DTO.Handlers;
using ChessGame.Model.DTO.Messages;
using ChessGame.Model.Moves;
using ChessGame.Services;
using ChessGame.Services.Implementations;
using ChessGame.Services.Implementations.Utils;
using ChessGame.Services.Interfaces;
using ChessGame.Services.Interfaces.Utils;
using ChessGame.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace ChessGame
{
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; }

        public App()
        {
            var services = new ServiceCollection();

            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<INetworkService, TcpNetworkService>();

            services.AddSingleton<IDtoResolver, DtoResolver>();
            services.AddSingleton<IMessageDispatcher, MessageDispatcher>();

            services.AddSingleton<IDtoMoveFactory, DtoMoveFactory>();
            services.AddSingleton<IBoardFactory, BoardFactory>();
            services.AddSingleton<IGameStateFactory, GameStateFactory>();

            services.AddSingleton<ILobbyService, LobbyService>();
            services.AddSingleton<IGameService, GameService>();
            services.AddSingleton<IChessRulesService, ChessRulesService>();

            services.AddTransient<IMessageHandler<DtoMove>, MoveHandler>();
            services.AddTransient<IMessageHandler<DtoStartGame>, StartGameHandler>();

            services.AddTransient<MainViewModel>();
            services.AddTransient<MenuViewModel>();

            services.AddTransient<SearchGameViewModel>();
            services.AddTransient<LobbyViewModel>();

            services.AddTransient<GameViewModel>();

            ServiceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainWindow = new MainWindow();

            var mainVM = ServiceProvider.GetRequiredService<MainViewModel>();
            mainWindow.DataContext = mainVM;

            mainWindow.Show();

            var nav = ServiceProvider.GetRequiredService<INavigationService>();
            nav.NavigateTo<MenuViewModel>();
        }
    }
}