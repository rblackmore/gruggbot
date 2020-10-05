// <copyright file="CommandHandler.cs" company="Ryan Blackmore">.
// Copyright © 2020 Ryan Blackmore. All rights Reserved.
// </copyright>

namespace Gruggbot.Core
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;
    using Discord;
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
            this.commandService.Log += this.CommandService_Log;
            this.commandService.CommandExecuted += this.CommandService_CommandExecuted;

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

            var result = await this.commandService.ExecuteAsync(context, argPos, this.serviceProvider)
                .ConfigureAwait(false);
        }

        private Task CommandService_CommandExecuted(Optional<CommandInfo> commandInfo, ICommandContext commandContext, IResult result)
        {
            if (!commandInfo.IsSpecified)
                return Task.CompletedTask;

            var template = "Command Executed - {module}:{commandName} by {user}:{channel}:{guild} - Message: {message}";

            var commandName = commandInfo.Value.Name;
            var module = commandInfo.Value.Module.Name;
            var userName = commandContext.User.Username;
            var channel = commandContext.Channel.Name;
            var guild = commandContext.Guild.Name;
            var message = commandContext.Message;

            this.logger.LogInformation(template, module, commandName, userName, channel, guild, message);

            return Task.CompletedTask;
        }

        private Task CommandService_Log(LogMessage msg)
        {
            var message = msg.Message;
            var source = msg.Source;

            var template = "Commands - {source}: {message}";

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
