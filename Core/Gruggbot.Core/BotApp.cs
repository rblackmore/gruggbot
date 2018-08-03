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
    /// <summary>
    /// Primary Class that Executes the Discord Bot. Call 
    /// </summary>
    public class BotApp
    {
        private const char PREFIX = '~';

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
            _discordClient.Connected += Client_Connected;

            await InstallCommands();

            string token = _configuration.GetSection("Token").Value;
            await _discordClient.LoginAsync(TokenType.Bot, token);
            await _discordClient.StartAsync();

            await Task.Delay(-1);
        }

        private Task Client_Connected()
        {
            _logger.LogInformation($"Connected as {_discordClient.CurrentUser.Username}");
            return Task.CompletedTask;
        }

        private async Task InstallCommands()
        {
            _discordClient.MessageReceived += HandleCommand;
            _commands.Log += Log;
            //Add Modules
            await _commands.AddModuleAsync<HelpModule>();
            await _commands.AddModuleAsync<InfoCommandModule>();
            await _commands.AddModuleAsync<FunStuffModule>();
            await _commands.AddModuleAsync<WarcraftModule>();
        }

        private async Task HandleCommand(SocketMessage msg)
        {
            if (!(msg is SocketUserMessage message))
                return;

            if (message.Author.IsBot)
                return;

            //First character of command after prefix
            int argPos = 0;

            bool hasPrefix = message.HasCharPrefix(PREFIX, ref argPos);
            bool hasMentionPrefix = message.HasMentionPrefix(_discordClient.CurrentUser, ref argPos);

            if (!hasPrefix && !hasMentionPrefix)
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