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
using Microsoft.Extensions.DependencyInjection;
using Gruggbot.Core.Helpers;

namespace Gruggbot.Core
{
    /// <summary>
    /// Primary Class that Executes the Discord Bot. Call 
    /// </summary>
    public class BotApp
    {
        private const char PREFIX = '~';
        private const string LOGPATH = "data/logs/bot.log";

        private IConfigurationRoot _configuration;
        private ILogger<BotApp> _logger;
        private IServiceProvider _serviceProvider;
        private DiscordSocketClient _discordClient;
        private CommandService _commands;
        private RandomMessages _randomMessages;

        public BotApp(ILogger<BotApp> logger, IConfigurationRoot configuration, DiscordSocketClient discordClient, IServiceProvider serviceProvider)
        {
            _configuration = configuration;
            _logger = logger;
            _serviceProvider = serviceProvider;
            _discordClient = discordClient;
            _commands = new CommandService();
            //ConfigureLogging();

        }

        public async Task Run()
        {
            _discordClient.Log += DiscordLogEvent;
            _discordClient.Connected += Client_Connected;

            _randomMessages = _serviceProvider.GetService<RandomMessages>();

            await InstallCommands(_serviceProvider);

            string token = _configuration.GetSection("Token").Value;
            if (String.IsNullOrEmpty(token))
            {
                _logger.LogError($"Token not valid {token}");
                return;
            }
            await _discordClient.LoginAsync(TokenType.Bot, token);
            await _discordClient.StartAsync();

            await Task.Delay(-1);
        }

        //private void ConfigureLogging()
        //{
        //    Log.Logger = new LoggerConfiguration()
        //        .MinimumLevel.Verbose()
        //        .WriteTo.Console()
        //        .WriteTo.File(LOGPATH, rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: LogEventLevel.Debug)
        //        .CreateLogger();
        //}


        private Task Client_Connected()
        {
            _logger.LogInformation($"Connected as {_discordClient.CurrentUser.Username}");
            return Task.CompletedTask;
        }

        private async Task InstallCommands(IServiceProvider serviceProvider)
        {
            _discordClient.MessageReceived += HandleCommand;
            _commands.Log += DiscordLogEvent;
            
            //Add Modules
            await _commands.AddModuleAsync<HelpModule>(serviceProvider);
            await _commands.AddModuleAsync<InfoCommandModule>(serviceProvider);
            await _commands.AddModuleAsync<FunStuffModule>(serviceProvider);
            await _commands.AddModuleAsync<WarcraftModule>(serviceProvider);
        }

        private async Task HandleCommand(SocketMessage msg)
        {
            if (!MessageContentCheckHelper.IsSocketUserMessage(msg, out SocketUserMessage message))
                return;

            if (!MessageContentCheckHelper.HasPrefix(_discordClient, message, PREFIX, out int argPos))
                return;

            //Create Command Context
            var context = new CommandContext(_discordClient, message);

            //Execute the command. (result does not indicate a return value,
            //rather an object stating if the command executed successfully
            var result = await _commands.ExecuteAsync(context, argPos, _serviceProvider);

            if (!result.IsSuccess)
                _logger.LogError("{@error} {@message}", result.ErrorReason, message.Content);
        }

        private Task DiscordLogEvent(LogMessage msg)
        {
            //_logger.LogInformation("{0}", msg.ToString());
            _logger.LogInformation(msg.ToString());
            return Task.CompletedTask;
        }
    }
}