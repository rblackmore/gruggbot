// <copyright file="ServiceCollectionExtensions.cs" company="Ryan Blackmore">.
// Copyright © 2020 Ryan Blackmore. All rights Reserved.
// </copyright>

namespace Gruggbot.DependencyInjection
{
    using System;

    using Discord.Commands;
    using Discord.WebSocket;
    using Gruggbot.CommandModules;
    using Gruggbot.CommandModules.TypeReaders;
    using Gruggbot.Configuration;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBot(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            services.AddSingleton<DiscordSocketClient>();
            services.ConfigureAndAddCommandServices(configuration);

            services.Configure<BotConfiguration>(configuration.GetSection(BotConfiguration.Bot));
            services.AddHostedService<BotApp>();

            services.Configure<RandomMessagesConfiguration>(configuration.GetSection(RandomMessagesConfiguration.RandomMessages));
            services.AddSingleton<RandomMessages>();

            services.AddTransient<ShadowlandsCountdownProvider>();

            return services;
        }

        private static IServiceCollection ConfigureAndAddCommandServices(this IServiceCollection services, IConfiguration configuration)
        {
            var commandConfig = configuration
                .GetSection(CommandHandlerConfiguration.CommandServiceConfig)
                .Get<CommandServiceConfig>();

            var commandService = new CommandService(commandConfig);

            commandService.AddTypeReader<LookupType>(new LookupTypeReader());
            commandService.AddTypeReader<TestOption>(new OptionsTypeReader());

            services.AddSingleton(commandService);
            services.AddSingleton<CommandHandler>();

            return services;
        }
    }
}