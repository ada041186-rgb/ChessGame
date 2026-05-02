// FILE: ChessInfrastructure/DependencyInjection.cs
using ChessApplication.DTO;
using ChessApplication.DTO.Messages;
using ChessApplication.Interfaces.Network;
using ChessInfrastructure.DTO.Factories;
using ChessInfrastructure.DTO.Handlers;
using ChessInfrastructure.Network;
using Microsoft.Extensions.DependencyInjection;

namespace ChessInfrastructure
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Registers all infrastructure-layer services: TCP networking, DTO resolution,
        /// message dispatch, DTO move factories and their handlers.
        /// </summary>
        public static IServiceCollection AddChessInfrastructure(this IServiceCollection services)
        {
            services.AddNetworking();
            services.AddDtoMoveFactories();
            services.AddMessageHandlers();

            return services;
        }

        private static IServiceCollection AddNetworking(this IServiceCollection services)
        {
            services.AddSingleton<INetworkService, TcpNetworkService>();
            services.AddSingleton<IDtoResolver, DtoResolver>();
            services.AddSingleton<IMessageDispatcher, MessageDispatcher>();

            return services;
        }

        private static IServiceCollection AddDtoMoveFactories(this IServiceCollection services)
        {
            services.AddSingleton<ISpecificDtoMoveFactory, DtoNormalMoveFactory>();
            services.AddSingleton<ISpecificDtoMoveFactory, DtoDoubleMoveFactory>();
            services.AddSingleton<ISpecificDtoMoveFactory, DtoEnPasssantMoveFactory>();
            services.AddSingleton<ISpecificDtoMoveFactory, DtoPromotionMoveFactory>();

            services.AddSingleton<IDtoMoveFactory, DtoMoveDispatcher>();

            return services;
        }

        private static IServiceCollection AddMessageHandlers(this IServiceCollection services)
        {
            services.AddTransient<IMessageHandler<DtoNormalMove>, NormalMoveHandler>();
            services.AddTransient<IMessageHandler<DtoDoubleMove>, DoubleMoveHandler>();
            services.AddTransient<IMessageHandler<DtoEnPassantMove>, DtoEnPassantMoveHandler>();
            services.AddTransient<IMessageHandler<DtoPromotionMove>, PromotionMoveHandler>();
            services.AddTransient<IMessageHandler<DtoStartGame>, StartGameHandler>();
            services.AddTransient<IMessageHandler<DtoResign>, ResignHandler>();

            return services;
        }
    }
}