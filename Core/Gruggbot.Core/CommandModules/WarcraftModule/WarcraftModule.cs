// <copyright file="WarcraftModule.cs" company="Ryan Blackmore">.
// Copyright © 2020 Ryan Blackmore. All rights Reserved.
// </copyright>

namespace Gruggbot.Core.CommandModules
{
    using System.Threading.Tasks;

    using Discord.Commands;

    [Summary("Warcraft")]
    public class WarcraftModule : ModuleBase
    {
        private ShadowlandsCountdownProvider countdownProvider;

        [Command("shadowlands")]
        [Summary("Provides a countdown Timer for World of Warcraft: Shadowlands Expansion")]
        [Alias("sl", "wow:sl", "shadowlambs")]
        public async Task ShadowlandsReleaseCommand()
        {
            this.countdownProvider = new ShadowlandsCountdownProvider(this.Context);

            await this.countdownProvider
                .SendShadowlandsReleaseCountdownMessagesAsync()
                .ConfigureAwait(false);
        }
    }
}
