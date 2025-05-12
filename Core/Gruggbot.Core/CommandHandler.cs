// <copyright file="CommandHandler.cs" company="Ryan Blackmore">.
// Copyright © 2020 Ryan Blackmore. All rights Reserved.
// </copyright>

namespace Gruggbot
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading.Tasks;
    using Discord;
    using Discord.Commands;
    using Discord.Commands.Builders;
    using Discord.WebSocket;
    using Gruggbot.Configuration;
    using Gruggbot.Extensions;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class CommandHandler
    {
        private readonly ILogger<CommandHandler> logger;
        private readonly BotConfiguration options;
        private readonly IServiceProvider serviceProvider;
        private readonly CommandService commandService;
        private readonly DiscordSocketClient discordClient;

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
            this.commandService.CommandExecuted += this.CommandExecuted;

            await this.commandService
                .AddModulesAsync(Assembly.GetExecutingAssembly(), this.serviceProvider)
                .ConfigureAwait(false);
        }

        private static Dictionary<string, string> BuildLogContext(
            CommandInfo commandInfo,
            ICommandContext commandContext,
            IResult result = null)
        {
            var logContext = new Dictionary<string, string>
            {
                ["module"] = commandInfo.Module.Name,
                ["commandName"] = commandInfo.Name,
                ["guild"] = commandContext.Guild.Name,
                ["channel"] = commandContext.Channel.Name,
                ["userName"] = commandContext.User.Username,
                ["messageContent"] = commandContext.Message.Content,
            };

            if (result != null)
            {
                logContext["errorType"] = result.Error.Value.ToString();
                logContext["errorReason"] = result.ErrorReason;
            }

            return logContext;
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

            _ = await this.commandService.ExecuteAsync(context, argPos, this.serviceProvider)
                .ConfigureAwait(false);
        }

        private Task CommandExecuted(Optional<CommandInfo> commandInfo,
            ICommandContext commandContext, IResult result)
        {
            if (result.IsSuccess)
                this.LogSuccessfulCommand(commandInfo.Value, commandContext);
            else
                this.LogUnsuccessfulCommand(commandInfo.Value, commandContext, result);

            return Task.CompletedTask;
        }

        private void LogSuccessfulCommand(CommandInfo commandInfo, ICommandContext commandContext)
        {
            var template = "Command Executed: {module}->{commandName}";

            var logContext = BuildLogContext(commandInfo, commandContext);

            using (this.logger.BeginScope(logContext))
            {
                this.logger.LogInformation(template, logContext["module"],
                    logContext["commandName"]);
            }
        }

        private void LogUnsuccessfulCommand(CommandInfo commandInfo, ICommandContext commandContext,
            IResult result)
        {
            var template = "Error Processing Command: {module}->{commandName}";

            var logContext = BuildLogContext(commandInfo, commandContext, result);

            using (this.logger.BeginScope(logContext))
            {
                this.logger.LogError(template, logContext["module"], logContext["commandName"]);
            }
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