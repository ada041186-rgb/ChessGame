using ChessApplication.Interfaces.Game;
using ChessApplication.Interfaces.Utils;
using ChessApplication.Services.Game;
using ChessApplication.Services.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace ChessApplication
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddChessApplication(this IServiceCollection services)
        {
            services.AddGameServices();
            services.AddUtilServices();

            return services;
        }

        private static IServiceCollection AddGameServices(this IServiceCollection services)
        {
            services.AddSingleton<IGameHistoryService, GameHistoryService>();
            services.AddSingleton<IGameService, GameService>();

            return services;
        }

        private static IServiceCollection AddUtilServices(this IServiceCollection services)
        {
            services.AddSingleton<ISettingsService, SettingsService>();
            services.AddSingleton<ILobbyService, LobbyService>();

            return services;
        }
    }
}