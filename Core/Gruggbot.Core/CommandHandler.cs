// <copyright file="CommandHandler.cs" company="Ryan Blackmore">.
// Copyright © 2020 Ryan Blackmore. All rights Reserved.
// </copyright>

namespace Gruggbot.Core
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;

    using Discord.Commands;
    using Discord.WebSocket;
    using Gruggbot.Core.Configuration;
    using Gruggbot.Core.Helpers;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class CommandHandler
    {
        private ILogger<CommandHandler> logger;
        private BotConfiguration options;
        private IServiceProvider serviceProvider;
        private CommandService commandService;
        private DiscordSocketClient discordClient;

        public CommandHandler(
            ILogger<CommandHandler> logger,
            IOptions<BotConfiguration> options,
            IServiceProvider serviceProvider,
            CommandService commandService,
            DiscordSocketClient discordClient)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            this.logger = logger;
            this.options = options.Value;
            this.serviceProvider = serviceProvider;
            this.commandService = commandService;
            this.discordClient = discordClient;
        }

        public async Task InitializeAsync()
        {
            this.discordClient.MessageReceived += this.HandleCommandAsync;

            await this.commandService
                .AddModulesAsync(Assembly.GetExecutingAssembly(), this.serviceProvider)
                .ConfigureAwait(false);
        }

        private async Task HandleCommandAsync(SocketMessage message)
        {
            if (message.Author.IsBot)
                return;

            if (!message.TryCastSocketUserMessage(out SocketUserMessage userMessage))
                return;

            var botUser = this.discordClient.CurrentUser;
            var prefix = this.options.Commands.Prefix;

            if (!userMessage.HasPrefix(botUser, prefix, out int argPos))
                return;

            var context = new CommandContext(this.discordClient, userMessage);

            await this.commandService.ExecuteAsync(context, argPos, this.serviceProvider)
                .ConfigureAwait(false);
        }
    }
}
