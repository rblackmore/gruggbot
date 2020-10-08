// <copyright file="WarcraftModule.cs" company="Ryan Blackmore">.
// Copyright © 2020 Ryan Blackmore. All rights Reserved.
// </copyright>

namespace Gruggbot.CommandModules
{
    using System.Threading.Tasks;

    using Discord.Commands;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    [Summary("Warcraft")]
    public class WarcraftModule : ModuleBase
    {
        private readonly ShadowlandsCountdownProvider countdownProvider;

        public WarcraftModule(ShadowlandsCountdownProvider countdownProvider)
        {
            this.countdownProvider = countdownProvider;
        }

        [Command("shadowlands")]
        [Summary("Provides a countdown Timer for World of Warcraft: Shadowlands Expansion")]
        [Alias("sl", "wow:sl", "shadowlambs")]
        public async Task ShadowlandsReleaseCommand()
        {
            await this.countdownProvider.SendImageLogoMessageAsync(this.Context.Channel).ConfigureAwait(false);
            await this.countdownProvider.SendCountdownTimeMessageAsync(this.Context.Channel).ConfigureAwait(false);
            await this.countdownProvider.SendPrePatchETAMessageAsync(this.Context.Channel).ConfigureAwait(false);
        }
    }
}
