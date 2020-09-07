using Discord.Commands;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Gruggbot.Core.CommandModules
{
    [Summary("Warcraft")]
    public class WarcraftModule : ModuleBase
    {
        private DateTime releaseDateUTC = new DateTime(2020, 10, 26, 23, 0, 0, DateTimeKind.Utc);
        private DateTime prePatchDateUTC = new DateTime(2020, 9, 22, 00, 0, 0, DateTimeKind.Utc);

        public async Task ShadowlandsRelease()
        {

            
            DateTime aestpatch = TimeZoneInfo.ConvertTimeFromUtc(prePatchDateUTC, aestZoneInfo);

            var releaseCountdown = releaseDateUTC - DateTime.UtcNow;
            var patchCountdown = prePatchDateUTC - DateTime.UtcNow;

            string releasemessage = $"World of Warcraft: Shadowlands Will be released in {releaseCountdown.Days} Days {releaseCountdown.Hours} hours {releaseCountdown.Minutes} minutes and {releaseCountdown.Seconds} seconds" +
                                    $"{Environment.NewLine}{aestRelease.ToLongDateString()} at {aestRelease.ToLongTimeString()} AEST";

            string patchETAmessage = $"Shadowlands Pre-Patch is estimated to release on {aestpatch.DayOfWeek} {aestpatch.Day} of {aestpatch:MMMM} {aestpatch.Year}";

            string image = $"data/Images/Shadowlands_Logo.png";
            string map = $"data/Images/ShadowlandsGlobalTimes.jpg";

            await Context.Channel.SendFileAsync(image);
            await Context.Channel.SendFileAsync(map, text: releasemessage);
            await Context.Channel.SendMessageAsync(patchETAmessage);
        }

        [Command("shadowlands")]
        [Summary("Provides a countdown Timer for World of Warcraft: Shadowlands Expansion")]
        [Alias("sl", "wow:sl", "shadowlambs")]
        public async Task SendShadowlandsReleaseCountdownMessagesAsync()
        {
            await SendImageLogoMessageAsync();
            await SendCountdownTimeMessageAsync();
            await SendPrePatchETAMessageAsync();

            string releaseMessage = GetShadowlandsReleaseCountdownMessage();
            string prePatchMessage = GetShadowlandsPrePatchReleaseDateMessage();

            
            await Context.Channel.SendMessageAsync(prePatchMessage);
        }
        private async Task SendImageLogoMessageAsync()
        {
            string expansionLogoImageLocation = GetImageLogoImageLocation();
            await Context.Channel.SendFileAsync(expansionLogoImageLocation);
        }

        private string GetImageLogoImageLocation()
        {
            //Should get this from a config or something.
            return $"data/Images/Shadowlands_Logo.png";
        }

        private async Task SendCountdownTimeMessageAsync()
        {
            string releaseMessage = GetReleaseCountdownMessage();
            string expansionReleaseMapImageLocation = GetReleaseMapImageLocation();
            await Context.Channel.SendFileAsync(expansionReleaseMapImageLocation, text: releaseMessage);
        }

        private string GetReleaseCountdownMessage()
        {
            TimeSpan releaseCountdownTimeSpan = CalculateReleaseCountdown();
            DateTime releaseDateAustralia = ConvertToAustralianEasternSummerTimeFromUTC(this.releaseDateUTC);





                return $"World of Warcraft: Shadowlands Will be released in {releaseCountdown.Days} Days {releaseCountdown.Hours} hours {releaseCountdown.Minutes} minutes and {releaseCountdown.Seconds} seconds" +
                                        $"{Environment.NewLine}{aestRelease.ToLongDateString()} at {aestRelease.ToLongTimeString()} AEST";
        }

        private TimeSpan CalculateReleaseCountdown()
        {
            return this.releaseDateUTC - DateTime.UtcNow;
        }

        private DateTime ConvertToAustralianEasternSummerTimeFromUTC(DateTime dateUTC)
        {
            TimeZoneInfo aedtZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("AUS Eastern Summer Time");
            return TimeZoneInfo.ConvertTimeFromUtc(dateUTC, aedtZoneInfo);
        }

        private string BuildReleaseMessage(TimeSpan countdown, DateTime releaseDate)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"World of Warcraft: Shadowlands Will be release in ");
            sb.Append($"{countdown.Days} Days, ");
            sb.Append($"{countdown.Hours} Hours, ");
            sb.Append($"{countdown.Minutes} Minutes, ");
            sb.Append($"{countdown.Seconds} Seconds, ");

            return sb.ToString();
        }

        private string GetReleaseMapImageLocation()
        {
            return $"data/Images/ShadowlandsGlobalTimes.jpg";
        }

        private async Task SendPrePatchETAMessageAsync()
        {
            throw new NotImplementedException();
        }

    }
}
