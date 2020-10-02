using Discord.Commands;
using Gruggbot.Core.DiscordExtensions;
using Microsoft.Extensions.Logging;
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
        private ILogger<HelpModule> _logger;
        private CommandService _commands;
        private IServiceProvider _map;

        public HelpModule(ILogger<HelpModule> logger, CommandService commands, IServiceProvider map)
        {
            _logger = logger;
            _commands = commands;
            _map = map;
        }

        [Command("help", RunMode = RunMode.Async), Summary("Displays a very helpful message")]
        public async Task Help()
        {
            //Obtain list of Modules, that contain executable commands by the calling user. That are not submodules, and are not hidden
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
            bool isModuleLookup = name.ToLower().Contains("module");
            string helpInformation;
            if (isModuleLookup)
                helpInformation = await HelpModuleLookup(name, Context, _map).ConfigureAwait(false);
            else
                helpInformation = await HelpCommandLookup(name, Context, _map).ConfigureAwait(false);

            await ReplyAsync(helpInformation).ConfigureAwait(false);
        }

        private async Task<string> HelpModuleLookup(string name, ICommandContext context, IServiceProvider map)
        {
            //var module =
            //    (from mody in await _commands.Modules.CheckConditions(context, map).ConfigureAwait(false)
            //     where mody.Name == name
            //     where !mody.Attributes.Any(att => att is HiddenAttribute)
            //     where !mody.Commands.Any(cmd => cmd.Attributes.Any(att => att is HiddenAttribute))
            //     select mody).FirstOrDefault();

            //var mod = _commands.Modules.CheckConditions(context, map).Result.FirstOrDefault(mi => mi.Name == name);


            IEnumerable<ModuleInfo> modules = await _commands.Modules.CheckConditions(Context, _map).ConfigureAwait(false);
            ModuleInfo module = modules.FirstOrDefault(m => m.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
            IEnumerable<string> commandStrings = await module?.Commands
                .Where(cmd => !cmd.Attributes.Any(att => att is HiddenAttribute))
                .GetCommandListStringAsync(Context, _map);

            StringBuilder sb = new StringBuilder();

            if (module != null)
            {
                sb.AppendLine($"```Markdown\n[{module.Name}]");
                sb.AppendLine(new string('=', module.Name.Length + 2));
                if (!String.IsNullOrEmpty(module.Summary))
                    sb.AppendLine($"> {module.Summary}\n```");
                if (module.Aliases.FirstOrDefault().Length > 0)
                    sb.AppendLine($"Use `{module.Aliases.FirstOrDefault()} <command>` to call commands from this Module");
                if (commandStrings.Count() > 0)
                    sb.AppendLine($"`{String.Join("`, `", commandStrings)}`");
                else
                    sb.AppendLine($"*No Commands Available*");

                sb.AppendLine($"```Markdown\nUse 'help <command> for more information\n```");
            }
            else
            {
                sb.AppendLine($"No Module foud called `{name}`");
            }

            return sb.ToString();

        }

        private async Task<string> HelpCommandLookup(string commandName, ICommandContext commandContext, IServiceProvider map = null)
        {
            IEnumerable<CommandInfo> cmds = await _commands.Commands.CheckConditions(commandContext, map).ConfigureAwait(false);
            CommandInfo command = cmds.FirstOrDefault(ci => ci.Name.Equals(commandName, StringComparison.InvariantCultureIgnoreCase));

            StringBuilder sb = new StringBuilder();

            if (command != null)
            {
                sb.AppendLine($"```Markdown\n[{command.Name}]");
                sb.AppendLine(new string('=', command.Name.Length + 2));
                if (!String.IsNullOrEmpty(command.Summary))
                    sb.AppendLine($"> {command.Summary}");
                sb.AppendLine($"```");
                sb.AppendLine($"**Aliases:** {String.Join(", ", command.Aliases)}");
                sb.Append($"**Usage:** {((command.Aliases.Count > 1) ? command.Aliases.ElementAt(1) : command.Aliases.First())} ");

                if (command.Parameters.Any())
                {
                    sb.AppendLine($"[{String.Join("] [", command.Parameters.Select(p => p.Name))}]");
                    sb.AppendLine();
                    sb.AppendLine("__Parameters__");
                    foreach (var para in command.Parameters)
                    {
                        sb.AppendLine($"**{para.Name}:** {para.Summary}");
                    }
                }
            }
            else
            {
                sb.AppendLine($"No Command Found named `{commandName}`");
            }

            return sb.ToString();
        }

    }
}