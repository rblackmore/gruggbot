using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gruggbot.Core.CommandModules
{
    [Summary("Warcraft")]
    public class WarcraftModule : ModuleBase
    {
        private readonly DateTime releaseUTC = new DateTime(2018, 8, 13, 22, 00, 00, DateTimeKind.Utc);

        [Command("bfa")]
        [Alias("BattleForAzeroth")]
        public async Task BattleForAzeroth([Summary("Timezone Offset Hours eg (10, -8.5)")] double offset = 10d)
        {

            if (offset > 14 || offset < -14)
            {
                await ReplyAsync($"Invalid Timezone Offset {offset} must be between 14.0 and -14.0");
                return;
            }


            int hours = Convert.ToInt32(Math.Floor(Math.Abs(offset)));
            int minutes = ((Math.Abs(offset) - hours) > 0d) ? 30 : 0;


            hours = (offset > -1) ? hours : hours * -1;

            if (hours < 0 && minutes > 0)
                hours -= 1;

            DateTime releaseTZ = TimeZoneInfo.ConvertTimeFromUtc(releaseUTC, TimeZoneInfo.CreateCustomTimeZone("TZ", new TimeSpan(hours, minutes, 0), "TZ", "TZ"));

            StringBuilder sb = new StringBuilder();

            string sign = (hours > -1) ? "+" : "";

            //string offset = "UTC " + sign + ":" + 

            sb.AppendLine($"Battle For Azeroth Releases on {releaseTZ.ToLongDateString()} at {releaseTZ.ToShortTimeString()} (UTC {sign}{offset})");
            TimeSpan countdown = releaseUTC - DateTime.UtcNow;
            sb.AppendLine(String.Format("Countdown: {0} Days, {1} Hours, {2} Minutes, {3} Seconds", countdown.Days, countdown.Hours, countdown.Minutes, countdown.Seconds));

            if (offset == 10d)
            {
                sb.AppendLine("```");
                sb.AppendLine($"For other timezones, provide the timezone offset, eg. ~bfa -8.5");
                sb.AppendLine("```");
            }


            await ReplyAsync(sb.ToString());

        }
    }
}
