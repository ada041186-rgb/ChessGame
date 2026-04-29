using ChessGame.Model;
using ChessGame.Model.Abstractions;
using ChessGame.Model.DTO.Handlers;
using ChessGame.Model.DTO.Messages;
using ChessGame.Model.Moves;
using ChessGame.Model.PromotionStrategies;
using ChessGame.Model.Rules;
using ChessGame.Services;
using ChessGame.Services.Implementations;
using ChessGame.Services.Implementations.Game;
using ChessGame.Services.Implementations.Network;
using ChessGame.Services.Implementations.Utils;
using ChessGame.Services.Implementations.Utils.PieceFactories;
using ChessGame.Services.Interfaces;
using ChessGame.Services.Interfaces.Utils;
using ChessGame.Services.Interfaces.Utils.PieceFactories;
using ChessGame.ViewModel;
using ChessGame.ViewModel.Game;
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

            services.AddSingleton<ISettingsService, SettingsService>();


            services.AddSingleton<IDtoResolver, DtoResolver>();
            services.AddSingleton<IMessageDispatcher, MessageDispatcher>();

            services.AddSingleton<IBoardFactory, BoardFactory>();
            services.AddSingleton<IGameStateFactory, GameStateFactory>();
            services.AddSingleton<IMoveFactory, MoveFactory>();

            services.AddSingleton<IPieceFactory, PieceFactory>();
            services.AddTransient<ISubPieceFactory, PawnFactory>();

            services.AddSingleton<IEndGameRulePipeline, EndGameRulePipeline>();

            services.AddTransient<IEndGameRule, CheckmateRule>();
            services.AddTransient<IEndGameRule, StalemateRule>();
            services.AddTransient<IEndGameRule, RepetitionRule>();
            services.AddTransient<IEndGameRule, InsufficientMaterial>();


            services.AddTransient<IPromotionStrategy, PromoteToKnightStrategy>();
            services.AddTransient<IPromotionStrategy, PromoteToBishopStrategy>();
            services.AddTransient<IPromotionStrategy, PromoteToQueenStrategy>();
            services.AddTransient<IPromotionStrategy, PromoteToRookStrategy>();

            services.AddSingleton<ILobbyService, LobbyService>();
            services.AddSingleton<IGameService, GameService>();
            services.AddSingleton<IChessRulesService, ChessRulesService>();
            services.AddSingleton<IGameHistoryService, GameHistoryService>();

            services.AddTransient<IMessageHandler<DtoStartGame>, StartGameHandler>();

            services.AddTransient<MainViewModel>();
            services.AddTransient<MenuViewModel>();
            services.AddTransient<EndResultViewModel>();
            services.AddTransient<SearchGameViewModel>();
            services.AddTransient<LobbyViewModel>();
            services.AddTransient<SettingsViewModel>();

            services.AddTransient<GameViewModel>();


            services.AddTransient<IMessageHandler<DtoNormalMove>, NormalMoveHandler>();
            services.AddTransient<IMessageHandler<DtoPromotionMove>, PromotionMoveHandler>();

            services.AddTransient<ISpecificDtoMoveFactory, DtoNormalMoveFactory>();
            services.AddTransient<ISpecificDtoMoveFactory, DtoPromotionMoveFactory>();
            services.AddTransient<IDtoMoveFactory, DtoMoveDispatcher>();

            ServiceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var settingsService = ServiceProvider.GetRequiredService<ISettingsService>();
            var settings = settingsService.Load();

            var mainWindow = new MainWindow();
            ApplySettings(mainWindow, settings);

            var mainVM = ServiceProvider.GetRequiredService<MainViewModel>();
            mainWindow.DataContext = mainVM;

            mainWindow.Show();

            var nav = ServiceProvider.GetRequiredService<INavigationService>();
            nav.NavigateTo<MenuViewModel>();
        }

        private void ApplySettings(Window window, SettingsData settings)
        {
            if (settings.IsFullScreen)
            {
                window.WindowStyle = WindowStyle.None;
                window.WindowState = WindowState.Maximized;
            }
            else
            {
                window.WindowStyle = WindowStyle.SingleBorderWindow;
                window.WindowState = WindowState.Normal;
                window.Width = settings.Width;
                window.Height = settings.Height;

                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }
        }
    }
}