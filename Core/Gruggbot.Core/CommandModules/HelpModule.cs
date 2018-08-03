using Discord.Commands;
using Gruggbot.Core.DiscordExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gruggbot.Core.CommandModules
{
    [Summary("Helpful Information about the commands available")]
    public class HelpModule : ModuleBase
    {
        private CommandService _commands;
        private IServiceProvider _map;

        public HelpModule(CommandService commands, IServiceProvider map)
        {
            _commands = commands;
            _map = map;
        }

        [Command("help", RunMode = RunMode.Async), Summary("Displays a very helpful message")]
        public async Task Help()
        {
            var modules =
                from module in await _commands.Modules.CheckConditions(Context, _map).ConfigureAwait(false)
                where !module.IsSubmodule
                where !module.Preconditions.Any(pc => pc is HiddenAttribute)
                select new
                {
                    Name = module.Name,
                    Summary = module.Summary,
                    SubModuleNames = module.Submodules.Select(sm => sm.Name),
                    SubModules = module.Submodules,
                    AliasCount = module.Aliases.Count,
                    Alias = module.Aliases.FirstOrDefault(), //TODO: check this out, can i use the full list or do i need to jsut get one?
                    Commands = module.Commands.Where(c => !c.Preconditions.Any(pc => pc is HiddenAttribute)).GetCommandListStringAsync(Context, _map).Result
                };

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"```Markdown\nAvailable Commands for {Context.Message.Author.Username}```");

            int i = 0;

            foreach (var mod in modules)
            {
                i++;
                sb.Append($"**{i}. {mod.Name}** - ");

                if (mod.Commands.Count() > 0)
                    sb.AppendLine($"`{String.Join("`, `", mod.Commands)}`");

                if (mod.SubModuleNames.Count() > 0)
                    sb.AppendLine($"*SubModules*: **{String.Join("**, **", mod.SubModuleNames)}**");

                if (mod.Commands.Count() == 0 && mod.SubModuleNames.Count() == 0)
                    sb.AppendLine("***Module Not Implemented Yet***");



            }
            sb.AppendLine("```Markdown\nUser 'help <command>' or 'help <module>' for more information\n```");
            await ReplyAsync(sb.ToString()).ConfigureAwait(false);

        }

        [Command("help", RunMode = RunMode.Async), Summary("Displays a very helpful message about a Command or Command Module")]
        public async Task Help([Summary("The Command or Command Module to get help for")] string name)
        {
            await ReplyAsync("**Coming Soon** Searching for Specific Command or Command Module help is not implemented yet");
        }

    }
}
