using System.Threading.Tasks;

using Discord.Commands;

namespace Gruggbot.Core.CommandModules
{
    [Summary("Warcraft")]
    public class WarcraftModule : ModuleBase
    {
        [Command("shadowlands")]
        [Summary("Provides a countdown Timer " +
            "for World of Warcraft: Shadowlands Expansion")]
        [Alias("sl", "wow:sl", "shadowlambs")]
        public async Task ShadowlandsReleaseCommand()
        {
            var countdownProvider = 
                new ShadowlandsCountdownProvider(this.Context);

            await countdownProvider
                .SendShadowlandsReleaseCountdownMessagesAsync();
        }
    }
}
