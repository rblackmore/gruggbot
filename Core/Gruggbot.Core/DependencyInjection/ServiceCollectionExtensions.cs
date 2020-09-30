
using Discord.Commands;
using Discord.WebSocket;

using Gruggbot.Core.Configuration;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Gruggbot.Core.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBot(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureBot(configuration);

            services.AddHostedService<BotApp>();
            services.AddSingleton<DiscordSocketClient>();
            services.AddSingleton<CommandService>();
            services.AddSingleton<RandomMessages>();

            return services;
        }

        private static IServiceCollection ConfigureBot(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<BotConfiguration>(configuration.GetSection(BotConfiguration.Bot));

            return services;
        }
    }
}
