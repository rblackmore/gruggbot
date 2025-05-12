// <copyright file="InfoCommandModule.cs" company="Ryan Blackmore">.
// Copyright © 2020 Ryan Blackmore. All rights Reserved.
// </copyright>

namespace Gruggbot.CommandModules
{
    using System;
    using System.Text;
    using System.Threading.Tasks;

    using Discord;
    using Discord.Commands;

    [Group("InfoModule")]
    [Alias("info")]
    [Summary("Provides Information about the Bot or Users")]
    [RequireOwner]
    public class InfoCommandModule : ModuleBase
    {
        [Command]
        public async Task Default()
        {
            await this.ReplyAsync("Hello, my name is Gruggbot").ConfigureAwait(false);
        }

        [Command("system")]
        [Summary("Sends DirectMessage with details about the host system (Only Available to the bot Owner)")]
        public async Task SysInfo()
        {
            var author = this.Context.Message.Author;
            StringBuilder sb = new StringBuilder();

            OperatingSystem os = Environment.OSVersion;

            sb.AppendLine("System Information");
            sb.AppendLine($"Machine Name: {Environment.MachineName}");
            sb.AppendLine($"OS Platform: {os.Platform}");
            sb.AppendLine($"OS Version: {os.VersionString}");

            await author.SendMessageAsync(sb.ToString()).ConfigureAwait(false);
        }

        [Command("userinfo")]
        [Summary("Returns info about the current user, or the user parameter, if one is parsed.")]
        [Alias("user", "whois")]
        public async Task UserInfo([Summary("The (optional) user to get info for")] IUser user = null)
        {
            var userInfo = user ?? this.Context.Client.CurrentUser;
            var author = this.Context.Message.Author;

            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"User Info for {userInfo.Username}#{userInfo.Discriminator}");
            sb.AppendLine($"ID: {userInfo.Id}");
            sb.AppendLine($"Bot: {userInfo.IsBot}");
            sb.AppendLine($"Status: {userInfo.Status}");
            // sb.AppendLine($"Current Activity: {userInfo.Activity}");
            sb.AppendLine($"Avatar URL: {userInfo.GetAvatarUrl()}");

            await author.SendMessageAsync(sb.ToString()).ConfigureAwait(false);
        }
    }
}