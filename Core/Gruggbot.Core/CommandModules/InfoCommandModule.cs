using Discord;using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gruggbot.Core.CommandModules
{
    [Group("info")]
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
        [RequireOwner]
        public async Task SysInfo()
        {
            await Context.Message.Author.SendMessageAsync($"System Information: {Environment.MachineName}");
        }

        [Command("userinfo"), Summary("Returns info about the current user, or the user parameter, if one is parsed")]
        [Alias("user", "whois")]
        public async Task UserInfo([Summary("The (optional) user to get info for")] IUser user = null)
        {
            var userInfo = user ?? Context.Client.CurrentUser;
            await ReplyAsync($"{userInfo.Username}#{userInfo.Discriminator}");
        }
    }
}