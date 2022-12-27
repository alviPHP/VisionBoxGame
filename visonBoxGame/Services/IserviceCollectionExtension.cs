using Microsoft.Extensions.DependencyInjection;

namespace visonBoxGame.Services
{
    public static class IserviceCollectionExtension
    {
        public static IServiceCollection AddDataServices(this IServiceCollection services)
        {
            services.AddScoped<IDeckService, DeckService>();
            services.AddScoped<IGameService, GameService>();
            services.AddScoped<IPlayerService, PlayerService>();
            services.AddScoped<IScoreService, ScoreService>();
            services.AddScoped<IStateMachineService, StateMachineService>();
            return services;
        }
    }
}
