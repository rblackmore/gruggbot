using Discord;
using Discord.WebSocket;
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
        private DiscordSocketClient _discordClient;

        public BotApp(ILogger<BotApp> logger, IConfigurationRoot configuration, DiscordSocketClient discordClient)
        {
            _configuration = configuration;
            _logger = logger;
            _discordClient = discordClient;
        }

        public async Task Run()
        {
            _discordClient.Log += Log;
            _discordClient.MessageReceived += MessageRecieved;

            string token = _configuration.GetSection("Token").Value;
            await _discordClient.LoginAsync(TokenType.Bot, token);
            await _discordClient.StartAsync();
        }

        private async Task MessageRecieved(SocketMessage message)
        {
            if (!message.Author.IsBot)
                _logger.LogInformation("Message Received from {@Username} in {@Channel}: {@Content}", message.Author.Username, message.Channel.Name, message.Content);


            if (message.Content == "!ping")
                await message.Channel.SendMessageAsync("Pong!!!");
        }

        private Task Log(LogMessage msg)
        {
            _logger.LogInformation("{0}", msg.ToString());
            return Task.CompletedTask;
        }
    }
}