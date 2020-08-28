using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace Gruggbot.Core.CommandModules
{
    [Summary("Warcraft")]
    public class WarcraftModule : ModuleBase
    {
        [Command("shadowlands")]
        [Summary("Provides a countdown Timer for World of Warcraft: Shadowlands Expansion")]
        [Alias("sl", "wow:sl", "shadowlambs")]
        public async Task ShadowlandsRelease()
        {

            DateTime release = new DateTime(2020, 10, 26, 23, 0, 0, DateTimeKind.Utc);
            DateTime patch = new DateTime(2020, 9, 22, 00, 0, 0, DateTimeKind.Utc);

            TimeZoneInfo aestZoneInfo = TimeZoneInfo.CreateCustomTimeZone("AEDT", TimeSpan.FromHours(10), "AUS Eastern Daylight Savings Time", "AUS Eastern Daylight Savings Time");

            DateTime aestRelease = TimeZoneInfo.ConvertTimeFromUtc(release, aestZoneInfo);
            DateTime aestpatch = TimeZoneInfo.ConvertTimeFromUtc(patch, aestZoneInfo);

            var releaseCountdown = release - DateTime.UtcNow;
            var patchCountdown = patch - DateTime.UtcNow;

            string releasemessage = $"World of Warcraft: Shadowlands Will be released in {releaseCountdown.Days} Days {releaseCountdown.Hours} hours {releaseCountdown.Minutes} minutes and {releaseCountdown.Seconds} seconds" +
                                    $"{Environment.NewLine}{aestRelease.ToLongDateString()} at {aestRelease.ToLongTimeString()} AEST";

            string patchETAmessage = $"Shadowlands Pre-Patch is estimated to release on {aestpatch.DayOfWeek} {aestpatch.Day} of {aestpatch:MMMM} {aestpatch.Year}";

            string image = $"data/Images/Shadowlands_Logo.png";
            string map = $"data/Images/ShadowlandsGlobalTimes.jpg";

            await Context.Channel.SendFileAsync(image);
            await Context.Channel.SendFileAsync(map, text: releasemessage);
            await Context.Channel.SendMessageAsync(patchETAmessage);
        }
    }
}
