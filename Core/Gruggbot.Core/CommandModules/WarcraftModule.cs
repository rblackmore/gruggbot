using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace Gruggbot.Core.CommandModules
{
    [Summary("Warcraft")]
    public class WarcraftModule : ModuleBase
    {
        [Command("classic")]
        [Summary("Provides a countdown Timer for Classic WoW Release")]
        [Alias("classic", "wowclassic", "classico")]
        public async Task ClassicRelase(params string[] args)
        {

            DateTime release = new DateTime(2019, 08, 26, 22, 0, 0, DateTimeKind.Utc);
            DateTime charCreate = new DateTime(2019, 8, 12, 22, 0, 0, DateTimeKind.Utc);

            TimeZoneInfo aestZoneInfo = TimeZoneInfo.CreateCustomTimeZone("AEST", TimeSpan.FromHours(10), "AUS Eastern Standard Time", "AUS Eastern Standard Time");

            DateTime aestRelease = TimeZoneInfo.ConvertTimeFromUtc(release, aestZoneInfo);
            DateTime aestCharCreate = TimeZoneInfo.ConvertTimeFromUtc(charCreate, aestZoneInfo);

            var releaseCountdown = release - DateTime.UtcNow;
            var charCreateCountdown = charCreate - DateTime.UtcNow;

            string message = $"World of Warcraft: Classic Will be released in {releaseCountdown.Days} Days {releaseCountdown.Hours} hours {releaseCountdown.Minutes} minutes and {releaseCountdown.Seconds} seconds {Environment.NewLine} on {aestRelease.ToLongDateString()} at {aestRelease.ToLongTimeString()} AEST";

            string image = $"data/Images/classic.png";

            foreach (var arg in args)
            {
                if (arg.Equals("map", StringComparison.CurrentCultureIgnoreCase))
                {
                    image = $"data/Images/ClassicGlobalTimes.jpg";
                }

                if (arg.Equals("cc", StringComparison.CurrentCultureIgnoreCase) || arg.Equals("charcreate", StringComparison.CurrentCultureIgnoreCase) || arg.Equals("charactercreation", StringComparison.CurrentCultureIgnoreCase))
                {
                    message = $"World of Warcraft: Classic Character creation begins in {charCreateCountdown.Days} Days {charCreateCountdown.Hours} hours {charCreateCountdown.Minutes} minutes and {charCreateCountdown.Seconds} seconds {Environment.NewLine} on {aestCharCreate.ToLongDateString()} at {aestCharCreate.ToLongTimeString()} AEST";
                }
            }

            await Context.Channel.SendFileAsync(image, text: message);
        }
    }
}
