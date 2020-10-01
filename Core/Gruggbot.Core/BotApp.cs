using System;
using System.Threading;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Gruggbot.Core.CommandModules;
using Gruggbot.Core.Configuration;
using Gruggbot.Core.Helpers;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Gruggbot.Core
{
    /// <summary>
    /// Primary Class that Executes the Discord Bot. Call 
    /// </summary>
    public class BotApp : IHostedService
    {
        private readonly BotConfiguration _options;
        private readonly ILogger<BotApp> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly DiscordSocketClient _discordClient;
        private readonly CommandService _commands;

        public BotApp(
            ILogger<BotApp> logger, 
            IOptions<BotConfiguration> options, 
            DiscordSocketClient discordClient,
            CommandService commands, 
            IServiceProvider serviceProvider)
        {
            _options = options.Value;
            _logger = logger;
            _serviceProvider = serviceProvider;
            _discordClient = discordClient;
            _commands = commands;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _discordClient.Log += DiscordLogEvent;
            _discordClient.Connected += Client_Connected;

            _serviceProvider.GetService<RandomMessages>().Setup();

            await InstallCommands(_serviceProvider);

            string token = _options.Token;

            if (String.IsNullOrEmpty(token))
            {
                _logger.LogError($"Token not valid {token}");
                return;
            }
            await _discordClient.LoginAsync(TokenType.Bot, token);
            await _discordClient.StartAsync();
        }  
        
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            this._logger.LogInformation("Host Stopping");
            await this._discordClient.LogoutAsync();
        }

        private Task Client_Connected()
        {
            _logger.LogInformation("Connected as: {0}", _discordClient.CurrentUser.Username);
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

        private async Task HandleCommand(SocketMessage message)
        {
            if (message.TryCastSocketUserMessage(out SocketUserMessage userMessage))
                return;

            if (!MessageContentCheckHelper.HasPrefix(_discordClient, userMessage, _options.Prefix, out int argPos))
                return;

            //Create Command Context
            var context = new CommandContext(_discordClient, userMessage);

            //Execute the command. (result does not indicate a return value,
            //rather an object stating if the command executed successfully
            var result = await _commands.ExecuteAsync(context, argPos, _serviceProvider);
            
            if (!result.IsSuccess)
                _logger.LogError("{@error} {@message}", result.ErrorReason, userMessage.Content);
        }

        private Task DiscordLogEvent(LogMessage msg)
        {
            _logger.LogInformation("DiscordClient: {0}", msg.Message);
            return Task.CompletedTask;
        }
    }
}