using Discord.WebSocket;

using Imgur.API.Authentication;

using Microsoft.Extensions.DependencyInjection;

namespace Gruggbot.Core.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBot(this IServiceCollection services)
        {
            services.AddSingleton<BotApp>();
            services.AddSingleton<DiscordSocketClient>();
            services.AddSingleton<RandomMessages>();

            services.AddBotServices();

            return services;
        }

        private static IServiceCollection AddBotServices(this IServiceCollection services)
        {
            services.AddSingleton(new ApiClient("8aba898176d1bfc"));

            return services;
        }
    }
}
