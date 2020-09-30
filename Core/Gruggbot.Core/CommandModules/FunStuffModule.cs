using System.Text;
using System.Threading.Tasks;

using Discord.Commands;

using Microsoft.Extensions.Logging;

namespace Gruggbot.Core.CommandModules
{
    [Summary("Random fun commands to play with")]
    public class FunStuffModule : ModuleBase
    {
        private readonly ILogger<FunStuffModule> _logger;

        public FunStuffModule(ILogger<FunStuffModule> logger)
        {
            _logger = logger;
        }

        [Command("say"), Summary("Echos a message.")]
        public async Task Say([Remainder, Summary("The text to echo")] string echo)
        {
            await ReplyAsync(echo);
        }

        [Command("8ball", RunMode = RunMode.Async), Summary("Ask a question, get an answer")]
        [RequireOwner]
        [Hidden]
        public async Task EightBall([Remainder(), Summary("the Question to answer")]string question)
        {
            await Task.CompletedTask;
        }

        [Command("mario"), Summary("It's-a-me Mario!")]
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


            await ReplyAsync(top.ToString());
            await ReplyAsync(bot.ToString());
        }

        [Command("hjälp", RunMode = RunMode.Async), Summary("no hablar español")]
        [Hidden]
        public async Task HjalpCmd()
        {
            await ReplyAsync("No Hablar Español");
        }

    }
}
