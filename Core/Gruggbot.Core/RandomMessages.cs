using Discord;
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
        private const float DEFAULTCHANCE = 50f;
        private const float CHANCEINCREMENT = 10f;
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
            _discordClient.MessageReceived += BananaReaction;

            Log.Information($"RandomMessages Initiated");
        }

        public async Task ShenanigansResponse(SocketMessage message)
        {
            Random rando = new Random();

            double execute = rando.NextDouble() * 100;

            if (!_app.IsSocketUserMessage(message, out SocketUserMessage userMessage))
                return;

            if (userMessage.Content.ToLowerInvariant().Contains("shenanigans"))
            {
                if (execute > _chance)
                {
                    _chance += CHANCEINCREMENT;
                    Log.Verbose($"Chance increased to {_chance}");
                    return;
                }

                _chance = DEFAULTCHANCE;


                var channel = userMessage.Channel;
                var author = userMessage.Author;

                await channel.SendMessageAsync($"Pistol whips {author.Mention}");
            }
        }

        public async Task BananaReaction(SocketMessage message)
        {

            Random rando = new Random();

            double execute = rando.NextDouble() * 100;

            if (!_app.IsSocketUserMessage(message, out SocketUserMessage userMessage))
                return;

            if (userMessage.Content.ToLowerInvariant().Contains("banana"))
            {
                if (execute > _chance)
                {
                    _chance += CHANCEINCREMENT;
                    Log.Verbose($"Chance increased to {_chance}");
                    return;
                }

                _chance = DEFAULTCHANCE;
                Log.Verbose($"Chance reset to {DEFAULTCHANCE}");

                await userMessage.AddReactionAsync(new Emoji("🍌"));
            }

        }
    }
}
