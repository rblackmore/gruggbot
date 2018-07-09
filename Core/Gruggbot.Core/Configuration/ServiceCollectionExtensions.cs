using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gruggbot.Core.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBot(this IServiceCollection services)
        {
            services.AddSingleton<BotApp>();
            services.AddSingleton<DiscordSocketClient>();

            return services;
        }
    }
}
