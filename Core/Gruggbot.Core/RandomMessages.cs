using Discord;
using Discord.WebSocket;
using Gruggbot.Core.Helpers;
using Microsoft.Extensions.Logging;
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

        public RandomMessages(BotApp app, DiscordSocketClient discordClient, ILogger<RandomMessages> logger, IServiceProvider serviceProvider)
        {
            _discordClient = discordClient;
            _logger = logger;
            _serviceProvider = serviceProvider;
            Setup();
        }

        private void Setup()
        {
            _discordClient.MessageReceived += ShenanigansResponse;
            _discordClient.MessageReceived += BananaReaction;
            _discordClient.MessageReceived += WelcomeBack;

            _logger.LogInformation($"RandomMessages Initiated");
        }

        private async Task WelcomeBack(SocketMessage message)
        {

            if (!MessageContentCheckHelper.IsSocketUserMessage(message, out SocketUserMessage userMessage))
                return;

            if (message.Content.ToLower().Contains("welcome back") && MessageContentCheckHelper.BotMentioned(_discordClient, userMessage))
            {
                var channel = userMessage.Channel;
                var author = userMessage.Author;

                await channel.SendMessageAsync($"Thank you {author.Mention}");
            }
        }

        public async Task ShenanigansResponse(SocketMessage message)
        {
            Random rando = new Random();

            double execute = rando.NextDouble() * 100;

            if (!MessageContentCheckHelper.IsSocketUserMessage(message, out SocketUserMessage userMessage))
                return;

            if (userMessage.Content.ToLowerInvariant().Contains("shenanigans"))
            {
                if (execute > _chance)
                {
                    _chance += CHANCEINCREMENT;
                    _logger.Log(LogLevel.Trace,$"Chance increased to {_chance}");
                    return;
                }

                _chance = DEFAULTCHANCE;


                var channel = userMessage.Channel;
                var author = userMessage.Author;

                await channel.SendMessageAsync($"Pistol whips {author.Mention}");

                _logger.LogInformation("{Author} was Pistol Whipped in {Channel}", author.Username, channel.Name);
            }
        }

        public async Task BananaReaction(SocketMessage message)
        {

            Random rando = new Random();

            double execute = rando.NextDouble() * 100;

            if (!MessageContentCheckHelper.IsSocketUserMessage(message, out SocketUserMessage userMessage))
                return;

            if (userMessage.Content.ToLowerInvariant().Contains("banana"))
            {
                if (execute > _chance)
                {
                    _chance += CHANCEINCREMENT;
                    _logger.Log(LogLevel.Trace, $"Chance increased to {_chance}");
                    return;
                }

                _chance = DEFAULTCHANCE;
                _logger.Log(LogLevel.Trace, $"Chance reset to {DEFAULTCHANCE}");

                await userMessage.AddReactionAsync(new Emoji("🍌"));

                _logger.LogInformation("Banana Reaction given to {Author} in {Channel}", userMessage.Author.Username, userMessage.Channel.Name);
            }

        }
    }
}
