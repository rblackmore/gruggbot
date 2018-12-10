using Discord;using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gruggbot.Core.CommandModules
{
    [Group("InfoModule")]
    [Alias("info")]
    [Summary("Provides Information about the Bot or Users")]
    public class InfoCommandModule : ModuleBase
    {
        [Command]
        [Hidden]
        public async Task Default()
        {
            await ReplyAsync("Hello, my name is Gruggbot");
        }

        [Command("system")]
        [Summary("Sends DirectMessage with details about the host system (Only Available to the bot Owner)")]
        [RequireOwner]
        public async Task SysInfo()
        {
            var author = Context.Message.Author;
            StringBuilder sb = new StringBuilder();

            OperatingSystem OS = Environment.OSVersion;

            sb.AppendLine("System Information");
            sb.AppendLine($"Machine Name: {Environment.MachineName}");
            sb.AppendLine($"OS Platform: {OS.Platform}");
            sb.AppendLine($"OS Version: {OS.VersionString}");

            await author.SendMessageAsync(sb.ToString());

            //await Context.Message.Author.SendMessageAsync($"System Information: {Environment.MachineName}"); 
        }

        [Command("userinfo"), Summary("Returns info about the current user, or the user parameter, if one is parsed")]
        [Alias("user", "whois")]
        public async Task UserInfo([Summary("The (optional) user to get info for")] IUser user = null)
        {
            var userInfo = user ?? Context.Client.CurrentUser;
            var author = Context.Message.Author;

            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"User Info for {userInfo.Username}#{userInfo.Discriminator}");
            sb.AppendLine($"ID: {userInfo.Id}");
            sb.AppendLine($"Bot: {userInfo.IsBot}");
            sb.AppendLine($"Status: {userInfo.Status}");
            sb.AppendLine($"Currently Playing: {userInfo.Game}");
            sb.AppendLine($"Avatar URL: {userInfo.GetAvatarUrl()}");

            await author.SendMessageAsync(sb.ToString());
        }
    }
}