using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gruggbot.Core
{
    public class RandomMessages
    {
        private const float DEFAULTCHANCE = 5f;
        private const float CHANCEINCREMENT = 5f;
        private float _chance = DEFAULTCHANCE;

        private DiscordSocketClient _discordClient;
        private ILogger<RandomMessages> _logger;
        private IServiceProvider _serviceProvider;
        private BotApp _app;

        public RandomMessages(BotApp app, DiscordSocketClient discordClient, ILogger<RandomMessages> logger, IServiceProvider serviceProvider)
        {
            _discordClient = discordClient;
            _logger = logger;
            _serviceProvider = serviceProvider;
            _app = app;
            Setup();
        }

        private void Setup()
        {
            _discordClient.MessageReceived += ShenanigansResponse;
            Log.Information($"RandomMessages Initiated");
        }

        public async Task ShenanigansResponse(SocketMessage message)
        {
            Random rando = new Random();

            double execute = rando.NextDouble() * 100;

            if (!_app.IsSocketUserMessage(message, out SocketUserMessage userMessage))
                return;

            if (execute > _chance)
            {
                _chance += CHANCEINCREMENT;
                Log.Information($"Chance increased to {_chance}");
                return;
            }

            _chance = DEFAULTCHANCE;

            if (userMessage.Content.ToLowerInvariant().Contains("shenanigans"))
            {
                var channel = userMessage.Channel;
                var author = userMessage.Author;

                await channel.SendMessageAsync($"Pistol whips {author.Mention}");
            }
        }
    }
}
