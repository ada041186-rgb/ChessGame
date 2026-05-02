// FILE: ChessLibrary/DependencyInjection.cs
using ChessLibrary.Board;
using ChessLibrary.Factories;
using ChessLibrary.Factories.PieceFactories;
using ChessLibrary.Game;
using ChessLibrary.Game.Factories;
using ChessLibrary.Moves.Factories;
using ChessLibrary.Moves.Strategies;
using ChessLibrary.Moves.Strategies.BishopStrategies;
using ChessLibrary.Moves.Strategies.KingStrategies;
using ChessLibrary.Moves.Strategies.KnightStrategies;
using ChessLibrary.Moves.Strategies.PawnStrategies;
using ChessLibrary.Moves.Strategies.QueenStrategies;
using ChessLibrary.Moves.Strategies.RookStrategies;
using ChessLibrary.Rules;
using ChessLibrary.Rules.GameEnd;
using ChessLibrary.Rules.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace ChessLibrary
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddChessDomain(this IServiceCollection services)
        {
            services.AddFactories();
            services.AddMoveStrategies();
            services.AddRules();

            return services;
        }

        private static IServiceCollection AddFactories(this IServiceCollection services)
        {
            services.AddTransient<ISubPieceFactory, BishopFactory>();
            services.AddTransient<ISubPieceFactory, KingFactory>();
            services.AddTransient<ISubPieceFactory, KnightFactory>();
            services.AddTransient<ISubPieceFactory, PawnFactory>();
            services.AddTransient<ISubPieceFactory, QueenFactory>();
            services.AddTransient<ISubPieceFactory, RookFactory>();

            services.AddSingleton<IPieceCounterStrategy, StandardPieceCounter>();
            services.AddSingleton<IPieceFactory, PieceFactory>();
            services.AddSingleton<IBoardFactory, BoardFactory>();

            services.AddSingleton<IMoveFactory, MoveFactory>();

            services.AddSingleton<IGameStateFactory, GameStateFactory>();

            return services;
        }

        private static IServiceCollection AddMoveStrategies(this IServiceCollection services)
        {
            services.AddSingleton<IMoveStrategy, BishopMoveStrategy>();
            services.AddSingleton<IMoveStrategy, KingMoveStrategy>();
            services.AddSingleton<IMoveStrategy, KnightMoveStrategy>();

            services.AddSingleton<IMoveStrategy, PawnForwardStrategy>();
            services.AddSingleton<IMoveStrategy, PawnCaptureStrategy>();
            services.AddSingleton<IMoveStrategy, PawnDoubleStrategy>();
            services.AddSingleton<IMoveStrategy, PawnEnPassantStrategy>();

            services.AddSingleton<IMoveStrategy, QueenMoveStrategy>();
            services.AddSingleton<IMoveStrategy, RookMoveStrategy>();

            services.AddSingleton<IMoveService, MoveService>();

            return services;
        }

        private static IServiceCollection AddRules(this IServiceCollection services)
        {
            services.AddSingleton<IChessRulesEvaluator, ChessValidator>();

            services.AddSingleton<IEndGameRule, CheckmateRule>();
            services.AddSingleton<IEndGameRule, StalemateRule>();
            services.AddSingleton<IEndGameRule, InsufficientMaterialRule>();
            services.AddSingleton<IEndGameRule, RepetitionRule>();

            services.AddSingleton<IEndGameRulePipeline, EndGameEvaluator>();

            return services;
        }
    }
}