namespace Gruggbot.Core.CommandModules
{
    using System;
    using System.Text;
    using System.Threading.Tasks;

    using Discord.Commands;
    using TimeZoneConverter;

    internal class ShadowlandsCountdownProvider
    {
        private readonly DateTime releaseDateUTC =
            new DateTime(2020, 10, 26, 23, 0, 0, DateTimeKind.Utc);

        private readonly DateTime prePatchDateUTC =
            new DateTime(2020, 10, 13, 00, 0, 0, DateTimeKind.Utc);

        private ICommandContext Context { get; set; }

        public ShadowlandsCountdownProvider(ICommandContext context)
        {
            this.Context = context;
        }

        public async Task SendShadowlandsReleaseCountdownMessagesAsync()
        {
            await SendImageLogoMessageAsync();
            await SendCountdownTimeMessageAsync();
            await SendPrePatchETAMessageAsync();
        }

        private async Task SendImageLogoMessageAsync()
        {
            string expansionLogoImageLocation =
                $"data/Images/Shadowlands_Logo.png";

            await Context.Channel.SendFileAsync(expansionLogoImageLocation);
        }

        private async Task SendCountdownTimeMessageAsync()
        {
            string releaseMessage = BuildReleaseCountdownMessage();

            string expansionReleaseMapImageLocation =
                $"data/Images/ShadowlandsGlobalTimes.jpg";

            await Context.Channel
                .SendFileAsync(
                expansionReleaseMapImageLocation,
                text: releaseMessage
            );
        }

        private string BuildReleaseCountdownMessage()
        {
            if (IsReleased())
            {
                return "The Gates are Open, World of Warcraft: Shadowlands is out, go play!!!";
            }

            TimeSpan releaseCountdown = CalculateReleaseCountdown();

            DateTime releaseDateAustralia =
                ConvertToAUSEasternStandardTimeFromUTC(this.releaseDateUTC);

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

        private async Task SendPrePatchETAMessageAsync()
        {
            string prepatchMessage = BuildPrePatchETAMessage();
            await Context.Channel.SendMessageAsync(prepatchMessage);
        }

        private string BuildPrePatchETAMessage()
        {
            DateTime prepatchDateAustralia =
                ConvertToAUSEasternStandardTimeFromUTC(this.prePatchDateUTC);

            StringBuilder sb = new StringBuilder();

            sb.Append($"Shadowlands Pre-Patch 9.0 is estimated to release on ");
            sb.Append($"{prepatchDateAustralia.ToLongDateString()}");

            return sb.ToString();
        }
    }
}
