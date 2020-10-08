// <copyright file="BotApp.cs" company="Ryan Blackmore">.
// Copyright © 2020 Ryan Blackmore. All rights Reserved.
// </copyright>

namespace Gruggbot
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Discord;
    using Discord.WebSocket;
    using Gruggbot.Configuration;
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
            await this.discordClient.LogoutAsync()
                .ConfigureAwait(false);
        }

        private Task Client_Connected()
        {
            this.logger
                .LogInformation("Connected as: {botname}", this.discordClient.CurrentUser.Username);

            return Task.CompletedTask;
        }

        private Task DiscordLogEvent(LogMessage msg)
        {
            var message = msg.Message;
            var source = msg.Source;

            var template = "DiscordClient - {source}: {message}";

            switch (msg.Severity)
            {
                case LogSeverity.Critical:
                    this.logger.LogCritical(msg.Exception, template, source, message);
                    break;
                case LogSeverity.Error:
                    this.logger.LogError(msg.Exception, template, source, message);
                    break;
                case LogSeverity.Debug:
                    this.logger.LogDebug(msg.Exception, template, source, message);
                    break;
                case LogSeverity.Warning:
                    this.logger.LogWarning(msg.Exception, template, source, message);
                    break;
                case LogSeverity.Info:
                    this.logger.LogInformation(msg.Exception, template, source, message);
                    break;
                case LogSeverity.Verbose:
                    this.logger.LogTrace(msg.Exception, template, source, message);
                    break;
                default:
                    break;
            }

            return Task.CompletedTask;
        }
    }
}