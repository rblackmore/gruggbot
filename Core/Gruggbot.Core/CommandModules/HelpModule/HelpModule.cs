namespace Gruggbot.Core.CommandModules
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Discord.Commands;
    using Gruggbot.Core.DiscordExtensions;
    using Microsoft.Extensions.Logging;

    [Summary("Helpful Information about the commands available")]
    public class HelpModule : ModuleBase
    {
        private ILogger<HelpModule> logger;
        private CommandService commandService;
        private IServiceProvider serviceProvider;

        public HelpModule(ILogger<HelpModule> logger, CommandService commands, IServiceProvider serviceProvider)
        {
            this.logger = logger;
            this.commandService = commands;
            this.serviceProvider = serviceProvider;
        }

        [Command("help")]
        [Summary("Displays a very helpful message")]
        public async Task Help()
        {
            var modules = this.GetAvailableTopLevelModuleHelpInfo();

            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"```Markdown\nAvailable Commands for {this.Context.Message.Author.Username}```");

            int i = 0;

            foreach (var mod in modules)
            {
                i++;
                sb.Append($"**{i}. {mod.Name}** - ");

                if (mod.Commands.Any())
                    sb.AppendLine($"`{string.Join("`, `", mod.Commands)}`");

                if (mod.SubModuleNames.Any())
                    sb.AppendLine($"*SubModules*: **{string.Join("**, **", mod.SubModuleNames)}**");

                if (mod.Commands.Any() && mod.SubModuleNames.Any())
                    sb.AppendLine("***Module Not Implemented Yet***");
            }

            sb.AppendLine("```Markdown\nUser 'help <command>' or 'help <module>' for more information\n```");

            await this.ReplyAsync(sb.ToString()).ConfigureAwait(false);
        }

        private IEnumerable<ModuleHelpInfoModel> GetAvailableTopLevelModuleHelpInfo()
        {
            return this.commandService.Modules
                 .Where(mod => !mod.IsSubmodule)
                 .Where(mod => !mod.IsHidden())
                 .CheckConditions(this.Context, this.serviceProvider)
                 .GetAwaiter().GetResult()
                 .Select(mi => ModuleHelpInfoModel.CreateFromModuleInfo(mi, this.Context, this.serviceProvider));
        }

        [Command("help")]
        [Summary("Displays a very helpful message about a Command or Command Module")]
        internal async Task Help([Summary("The Command or Command Module to get help for")] string name)
        {
            bool isModuleLookup = name.Contains("module", StringComparison.InvariantCultureIgnoreCase);
            string helpInformation;
            if (isModuleLookup)
                helpInformation = await HelpModuleLookup(name, Context, serviceProvider).ConfigureAwait(false);
            else
                helpInformation = await HelpCommandLookup(name, Context, serviceProvider).ConfigureAwait(false);

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


            IEnumerable<ModuleInfo> modules = await commandService.Modules.CheckConditions(Context, serviceProvider).ConfigureAwait(false);
            ModuleInfo module = modules.FirstOrDefault(m => m.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
            IEnumerable<string> commandStrings = await module?.Commands
                .Where(cmd => !cmd.Attributes.Any(att => att is HiddenAttribute))
                .GetCommandListStringAsync(Context, serviceProvider);

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
            IEnumerable<CommandInfo> cmds = await commandService.Commands.CheckConditions(commandContext, map).ConfigureAwait(false);
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