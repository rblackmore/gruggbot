using Discord.WebSocket;
using Imgur.API.Authentication.Impl;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gruggbot.Core.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBot(this IServiceCollection services)
        {
            services.AddSingleton<BotApp>();
            services.AddSingleton<DiscordSocketClient>();

            services.AddBotServices();

            return services;
        }

        private static IServiceCollection AddBotServices(this IServiceCollection services)
        {
            services.AddSingleton(new ImgurClient("8aba898176d1bfc"));

            return services;
        }
    }
}
