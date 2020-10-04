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

            var commandConfig = configuration
                .GetSection(CommandHandlerConfiguration.CommandServiceConfig)
                .Get<CommandServiceConfig>();

            services.Configure<BotConfiguration>(configuration.GetSection(BotConfiguration.Bot));

            services.AddSingleton<DiscordSocketClient>();
            services.AddSingleton(new CommandService(commandConfig));

            services.AddHostedService<BotApp>();
            services.AddSingleton<CommandHandler>();
            services.AddSingleton<RandomMessages>();
            services.AddTransient<ShadowlandsCountdownProvider>();

            return services;
        }
    }
}