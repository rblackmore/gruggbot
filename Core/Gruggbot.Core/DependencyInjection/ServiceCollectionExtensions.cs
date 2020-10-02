// <copyright file="ServiceCollectionExtensions.cs" company="Ryan Blackmore">.
// Copyright © 2020 Ryan Blackmore. All rights Reserved.
// </copyright>

namespace Gruggbot.Core.DependencyInjection
{
    using System;
    using Discord.Commands;
    using Discord.WebSocket;
    using Gruggbot.Core.CommandModules;
    using Gruggbot.Core.Configuration;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBot(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            services.ConfigureBot(configuration);

            services.AddHostedService<BotApp>();

            services.AddSingleton<DiscordSocketClient>();
            services.AddSingleton<CommandService>();
            services.AddSingleton<CommandHandler>();

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