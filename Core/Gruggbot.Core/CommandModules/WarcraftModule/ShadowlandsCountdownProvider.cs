namespace Gruggbot.Core.CommandModules
{
    using System;
    using System.Text;
    using System.Threading.Tasks;

    using Discord;
    using TimeZoneConverter;

    public class ShadowlandsCountdownProvider
    {
        private readonly DateTime releaseDateUTC =
            new DateTime(2020, 10, 26, 23, 0, 0, DateTimeKind.Utc);

        private readonly DateTime prePatchDateUTC =
            new DateTime(2020, 10, 13, 00, 0, 0, DateTimeKind.Utc);

        internal async Task SendImageLogoMessageAsync(IMessageChannel channel)
        {
            string expansionLogoImageLocation =
                $"data/Images/Shadowlands_Logo.png";

            await channel.SendFileAsync(expansionLogoImageLocation).ConfigureAwait(false);
        }

        internal async Task SendCountdownTimeMessageAsync(IMessageChannel channel)
        {
            string releaseMessage = this.BuildReleaseCountdownMessage();

            string expansionReleaseMapImageLocation =
                $"data/Images/ShadowlandsGlobalTimes.jpg";

            await channel.SendFileAsync(expansionReleaseMapImageLocation, text: releaseMessage)
                .ConfigureAwait(false);
        }

        internal async Task SendPrePatchETAMessageAsync(IMessageChannel channel)
        {
            string prepatchMessage = this.BuildPrePatchETAMessage();
            await channel.SendMessageAsync(prepatchMessage).ConfigureAwait(false);
        }

        private string BuildReleaseCountdownMessage()
        {
            if (this.IsReleased())
            {
                return "The Gates are Open, World of Warcraft: Shadowlands is out, go play!!!";
            }

            TimeSpan releaseCountdown = this.CalculateReleaseCountdown();

            DateTime releaseDateAustralia =
                this.ConvertToAUSEasternStandardTimeFromUTC(this.releaseDateUTC);

            StringBuilder sb = new StringBuilder();

            sb.Append($"World of Warcraft: Shadowlands Will be release in ");
            sb.Append($"{releaseCountdown.Days} Days, ");
            sb.Append($"{releaseCountdown.Hours} Hours, ");
            sb.Append($"{releaseCountdown.Minutes} Minutes, ");
            sb.Append($"{releaseCountdown.Seconds} Seconds, ");
            sb.Append(Environment.NewLine);
            sb.Append($"On {releaseDateAustralia.ToLongDateString()} ");
            sb.Append($"at {releaseDateAustralia.ToLongTimeString()} ");
            sb.Append($"Australian Eastern Time");

            return sb.ToString();
        }

        private TimeSpan CalculateReleaseCountdown()
        {
            return this.releaseDateUTC - DateTime.UtcNow;
        }

        private bool IsReleased()
        {
            return this.releaseDateUTC < DateTime.UtcNow;
        }

        private DateTime ConvertToAUSEasternStandardTimeFromUTC(DateTime dateUTC)
        {
            // TZConvert will get the appropriate TimeZoneInfo, no matter which OS we're running on.
            TimeZoneInfo aestZoneInfo = TZConvert.GetTimeZoneInfo("Australia/Sydney");

            return TimeZoneInfo.ConvertTimeFromUtc(dateUTC, aestZoneInfo);
        }

        private string BuildPrePatchETAMessage()
        {
            DateTime prepatchDateAustralia =
                this.ConvertToAUSEasternStandardTimeFromUTC(this.prePatchDateUTC);

            StringBuilder sb = new StringBuilder();

            sb.Append($"Shadowlands Pre-Patch 9.0 is estimated to release on ");
            sb.Append($"{prepatchDateAustralia.ToLongDateString()}");

            return sb.ToString();
        }
    }
}
