// <copyright file="FunStuffModule.cs" company="Ryan Blackmore">.
// Copyright © 2020 Ryan Blackmore. All rights Reserved.
// </copyright>

namespace Gruggbot.Core.CommandModules
{
    using System.Text;
    using System.Threading.Tasks;

    using Discord.Commands;
    using Microsoft.Extensions.Logging;

    [Summary("Random fun commands to play with")]
    public class FunStuffModule : ModuleBase
    {
        private readonly ILogger<FunStuffModule> logger;

        public FunStuffModule(ILogger<FunStuffModule> logger)
        {
            this.logger = logger;
        }

        [Command("say")]
        [Summary("Echos a message.")]
        public async Task Say([Remainder, Summary("The text to echo")] string echo)
        {
            await this.ReplyAsync(echo).ConfigureAwait(false);
        }

        [Command("mario")]
        [Summary("It's-a-me, Mario!")]
        public async Task Mario()
        {
            StringBuilder top = new StringBuilder();
            StringBuilder bot = new StringBuilder();

            top.AppendLine(":black_circle: :black_circle: :black_circle: :red_circle: :red_circle: :red_circle: :red_circle: :red_circle: :red_circle:");
            top.AppendLine(":black_circle: :black_circle: :red_circle: :red_circle: :red_circle: :red_circle: :red_circle: :red_circle: :red_circle: :red_circle: :red_circle:");
            top.AppendLine(":black_circle: :black_circle: :chestnut: :chestnut: :chestnut: :chestnut: :cookie: :chestnut: :cookie:");
            top.AppendLine(":black_circle: :chestnut: :cookie: :chestnut: :cookie: :cookie: :cookie: :chestnut: :cookie: :cookie: :cookie:");
            top.AppendLine(":black_circle: :chestnut: :cookie: :chestnut: :chestnut: :cookie: :cookie: :cookie: :chestnut: :cookie: :cookie: :cookie:");
            top.AppendLine(":black_circle: :chestnut: :chestnut: :cookie: :cookie: :cookie: :cookie: :chestnut: :chestnut: :chestnut: :chestnut:");
            top.AppendLine(":black_circle: :black_circle: :black_circle: :cookie: :cookie: :cookie: :cookie: :cookie: :cookie: :cookie:");
            top.AppendLine(":black_circle: :black_circle: :chestnut: :chestnut: :red_circle: :chestnut: :chestnut: :chestnut:");

            bot.AppendLine(":black_circle: :chestnut: :chestnut: :chestnut: :red_circle: :chestnut: :chestnut: :red_circle: :chestnut: :chestnut: :chestnut:");
            bot.AppendLine(":chestnut: :chestnut: :chestnut: :chestnut: :red_circle: :red_circle: :red_circle: :red_circle: :chestnut: :chestnut: :chestnut: :chestnut:");
            bot.AppendLine(":cookie: :cookie: :chestnut: :red_circle: :cookie: :red_circle: :red_circle: :cookie: :red_circle: :chestnut: :cookie: :cookie:");
            bot.AppendLine(":cookie: :cookie: :cookie: :red_circle: :red_circle: :red_circle: :red_circle: :red_circle: :red_circle: :cookie: :cookie: :cookie:");
            bot.AppendLine(":cookie: :cookie: :red_circle: :red_circle: :red_circle: :red_circle: :red_circle: :red_circle: :red_circle: :red_circle: :cookie: :cookie:");
            bot.AppendLine(":black_circle: :black_circle: :red_circle: :red_circle: :red_circle: :black_circle: :black_circle: :red_circle: :red_circle: :red_circle:");
            bot.AppendLine(":black_circle: :chestnut: :chestnut: :chestnut: :black_circle: :black_circle: :black_circle: :black_circle: :chestnut: :chestnut: :chestnut:");
            bot.AppendLine(":chestnut: :chestnut: :chestnut: :chestnut: :black_circle: :black_circle: :black_circle: :black_circle: :chestnut: :chestnut: :chestnut: :chestnut:");

            await this.ReplyAsync(top.ToString()).ConfigureAwait(false);
            await this.ReplyAsync(bot.ToString()).ConfigureAwait(false);
        }

        [Command("hjälp")]
        [Hidden]
        public async Task HjalpCmd()
        {
            await this.ReplyAsync("No Hablar Español").ConfigureAwait(false);
        }
    }
}