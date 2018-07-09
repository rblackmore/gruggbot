 using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Gruggbot.Core.CommandModules;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gruggbot.Core
{
    public class BotApp
    {
        private IConfigurationRoot _configuration;
        private ILogger<BotApp> _logger;
        private IServiceProvider _serviceProvider;
        private DiscordSocketClient _discordClient;
        private CommandService _commands;

        public BotApp(ILogger<BotApp> logger, IConfigurationRoot configuration, DiscordSocketClient discordClient, IServiceProvider serviceProvider)
        {
            _configuration = configuration;
            _logger = logger;
            _serviceProvider = serviceProvider;
            _discordClient = discordClient;
            _commands = new CommandService();
        }

        public async Task Run()
        {
            _discordClient.Log += Log;

            await InstallCommands();

            string token = _configuration.GetSection("Token").Value;
            await _discordClient.LoginAsync(TokenType.Bot, token);
            await _discordClient.StartAsync();

            await Task.Delay(-1);
        }

        private async Task InstallCommands()
        {
            _discordClient.MessageReceived += HandleCommand;

            //Add Modules
            await _commands.AddModuleAsync<InfoCommandModule>();
        }

        private async Task HandleCommand(SocketMessage msg)
        {
            if (!(msg is SocketUserMessage message))
                return;

            if (message.Author.IsBot)
                return;

            //First character of command after prefix
            int argPos = 0;

            if (!(message.HasCharPrefix('!', ref argPos) || !message.HasMentionPrefix(_discordClient.CurrentUser, ref argPos)))
                return;

            //Create Command Context
            var context = new CommandContext(_discordClient, message);

            //Execute the command. (result does not indicate a return value,
            //rather an object stating if the command executed successfully
            var result = await _commands.ExecuteAsync(context, argPos, _serviceProvider);

            if (!result.IsSuccess)
                _logger.LogError("{@error} {@message}", result.ErrorReason, message.Content);
        }

        private Task Log(LogMessage msg)
        {
            _logger.LogInformation("{0}", msg.ToString());
            return Task.CompletedTask;
        }
    }
}