// <copyright file="RandomMessages.cs" company="Ryan Blackmore">.
// Copyright © 2020 Ryan Blackmore. All rights Reserved.
// </copyright>

namespace Gruggbot.Core
{
    using System;
    using System.Threading.Tasks;

    using Discord;
    using Discord.WebSocket;
    using Gruggbot.Core.Configuration;
    using Gruggbot.Core.Helpers;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Class which provides random message responses to a discord server.
    /// </summary>
    public class RandomMessages
    {
        private readonly ILogger<RandomMessages> logger;
        private readonly RandomMessagesConfiguration options;
        private readonly DiscordSocketClient discordClient;

        private readonly Random rando = new Random();
        private float chancePercentage;

        public RandomMessages(
            ILogger<RandomMessages> logger,
            IOptions<RandomMessagesConfiguration> options,
            DiscordSocketClient discordClient)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            if (discordClient == null)
                throw new ArgumentNullException(nameof(discordClient));

            this.logger = logger;
            this.options = options.Value;
            this.discordClient = discordClient;

            this.chancePercentage = this.options.DefaultChance;
        }

        /// <summary>
        /// Registers several response delegates with The MessageReceived Event.
        /// </summary>
        public void Setup()
        {
            this.discordClient.MessageReceived += this.RandomResponses;
            this.discordClient.MessageReceived += this.GuaranteedResponses;
        }

        /// <summary>
        /// Responds to a User Message with one of several responses.
        /// Depending on message content, and current response chance.
        /// Only responds to User Messages.
        /// </summary>
        /// <param name="message">Received Message to respond to.</param>
        private async Task RandomResponses(SocketMessage message)
        {
            if (message.Author.IsBot)
                return;

            if (message.TryCastSocketUserMessage(out SocketUserMessage userMessage))
                return;

            if (!this.ShouldReact())
                return;

            await this.BananaReaction(userMessage).ConfigureAwait(false);

            await this.ShenanigansResponse(userMessage).ConfigureAwait(false);
        }

        /// <summary>
        /// Responds to a User Message with one of several responses.
        /// Depending on message content.
        /// Only Responds to User Messages.
        /// </summary>
        /// <param name="message">Received Message to respond to.</param>
        private async Task GuaranteedResponses(SocketMessage message)
        {
            if (message.Author.IsBot)
                return;

            if (message.TryCastSocketUserMessage(out SocketUserMessage userMessage))
                return;

            await this.WelcomeBack(userMessage).ConfigureAwait(false);
        }

        private async Task WelcomeBack(SocketUserMessage userMessage)
        {
            if (!userMessage.Content.Contains("welcome back", StringComparison.InvariantCultureIgnoreCase))
                return;

            if (userMessage.IsUserMentioned(this.discordClient.CurrentUser))
            {
                var channel = userMessage.Channel;
                var author = userMessage.Author;

                await channel.SendMessageAsync($"Thank you {author.Mention}").ConfigureAwait(false);
            }
        }

        private async Task ShenanigansResponse(SocketUserMessage userMessage)
        {
            if (userMessage.Content.Contains("shenanigans", StringComparison.InvariantCultureIgnoreCase))
            {
                await userMessage.Channel.SendMessageAsync($"Pistol whips {userMessage.Author.Mention}")
                    .ConfigureAwait(false);
            }
        }

        private async Task BananaReaction(SocketUserMessage userMessage)
        {
            if (userMessage.Content.Contains("banana", StringComparison.InvariantCultureIgnoreCase))
                await userMessage.AddReactionAsync(new Emoji("🍌")).ConfigureAwait(false);
        }

        #region Helper Methods
        private bool ShouldReact()
        {
            double roll = this.Roll();

            this.logger.LogTrace("Rolled: {0}", roll);

            if (roll > this.chancePercentage)
            {
                this.IncrementChance();
                return false;
            }

            this.ResetChance();
            return true;
        }

        private double Roll(int max = 100)
        {
            return this.rando.NextDouble() * max;
        }

        private void ResetChance()
        {
            this.chancePercentage = this.options.DefaultChance;
        }

        private void IncrementChance()
        {
            this.chancePercentage += this.options.ChanceIncrement;
        }
        #endregion
    }
}