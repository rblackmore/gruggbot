// <copyright file="BotApp.cs" company="Ryan Blackmore">.
// Copyright © 2020 Ryan Blackmore. All rights Reserved.
// </copyright>

namespace Gruggbot.Core
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Discord;
    using Discord.WebSocket;
    using Gruggbot.Core.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class BotApp : IHostedService
    {
        private readonly ILogger<BotApp> logger;
        private readonly BotConfiguration options;
        private readonly IServiceProvider serviceProvider;
        private readonly CommandHandler commandHandler;
        private readonly DiscordSocketClient discordClient;

        public BotApp(
            ILogger<BotApp> logger,
            IOptions<BotConfiguration> options,
            IServiceProvider serviceProvider,
            CommandHandler commandHandler,
            DiscordSocketClient discordClient)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            this.logger = logger;
            this.options = options.Value;
            this.serviceProvider = serviceProvider;
            this.commandHandler = commandHandler;
            this.discordClient = discordClient;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            this.discordClient.Log += this.DiscordLogEvent;
            this.discordClient.Connected += this.Client_Connected;

            this.serviceProvider.GetService<RandomMessages>().Setup();

            await this.commandHandler.InitializeAsync()
                .ConfigureAwait(false);

            string token = this.options.Token;

            if (string.IsNullOrEmpty(token))
            {
                this.logger.LogError($"Token not valid {token}");
                return;
            }

            await this.discordClient.LoginAsync(TokenType.Bot, token)
                .ConfigureAwait(false);

            await this.discordClient.StartAsync()
                .ConfigureAwait(false);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            this.logger.LogInformation("Host Stopping");

            await this.discordClient.LogoutAsync()
                .ConfigureAwait(false);
        }

        private Task Client_Connected()
        {
            this.logger
                .LogInformation("Connected as: {0}", this.discordClient.CurrentUser.Username);

            return Task.CompletedTask;
        }

        private Task DiscordLogEvent(LogMessage msg)
        {
            this.logger.LogInformation("DiscordClient: {0}", msg.Message);

            return Task.CompletedTask;
        }
    }
}