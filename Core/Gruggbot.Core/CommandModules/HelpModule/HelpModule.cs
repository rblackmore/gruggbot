namespace Gruggbot.CommandModules
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Discord.Commands;
    using Gruggbot.CommandModules.Model;
    using Gruggbot.Extensions;
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

        /// <summary>
        /// Displays a help message on Top Level modules and commands.
        /// </summary>
        /// <returns>An awaitable Task.</returns>
        [Command("help")]
        [Summary("Displays a very helpful message")]
        public async Task Help()
        {
            var modules = this.commandService.Modules
                .GetAvailableTopLevelModules(this.Context, this.serviceProvider)
                .GetAwaiter().GetResult()
                .Select(m => ModuleHelpInfoModel.CreateFromModuleInfo(m, this.Context, this.serviceProvider));

            var helpMessage = this.BuildHelpMessageString(modules);

            await this.ReplyAsync(helpMessage).ConfigureAwait(false);
        }

        /// <summary>
        /// Displays a help message on a specified command or module.
        /// </summary>
        /// <param name="name">Name of command or module.</param>
        /// <returns>An awaitable Task.</returns>
        [Command("help")]
        [Summary("Displays a very helpful message about a Command or Command Module")]
        internal async Task Help([Summary("The Command or Command Module to get help for")] string name)
        {
            if (await this.SendModuleHelpInfo(name).ConfigureAwait(false))
                return;

            if (await this.SendCommandHelpInfo(name).ConfigureAwait(false))
                return;

            await this.ReplyAsync($"No Module or Command found by name of `{name}`").ConfigureAwait(false);
        }

        /// <summary>
        /// Displays a help message on a specified command or module.
        /// </summary>
        /// <param name="name">Name of command or module.</param>
        /// <param name="lookupType">Search for Command or Module.</param>
        /// <returns>An awaitable Task.</returns>
        [Command("help")]
        [Summary("Displays a very helpful message about a Command or Command Module")]
        internal async Task Help(
            [Summary("The Command or Command Module to get help for")] string name,
            [Summary("Specify if searching for a `command` or a `module`")] LookupType lookupType)
        {
            if (lookupType == LookupType.Module)
            {
                if (await this.SendModuleHelpInfo(name).ConfigureAwait(false))
                    return;

                await this.ReplyAsync($"No Module found by name of `{name}`").ConfigureAwait(false);
                return;
            }

            if (lookupType == LookupType.Command)
            {
                if (await this.SendCommandHelpInfo(name).ConfigureAwait(false))
                    return;

                await this.ReplyAsync($"No Command found by name of `{name}`").ConfigureAwait(false);
                return;
            }
        }

        private async Task<bool> SendModuleHelpInfo(string name)
        {
            name = name.Replace("module", string.Empty, StringComparison.InvariantCultureIgnoreCase);

            var module = this.commandService.Modules
                .CheckConditions(this.Context, this.serviceProvider)
                .GetAwaiter().GetResult()
                .FirstOrDefault(m => m.Name.Equals($"{name}module", StringComparison.InvariantCultureIgnoreCase));

            if (module == null)
                return false;

            if (module.IsHidden())
                return false;

            var commandStrings = await module.Commands
                .Where(c => !c.IsHidden())
                .GetCommandListStringAsync(this.Context, this.serviceProvider)
                .ConfigureAwait(false);

            var helpMessage = this.BuildModuleHelpMessageString(module);

            await this.ReplyAsync(helpMessage).ConfigureAwait(false);

            return true;
        }

        private async Task<bool> SendCommandHelpInfo(string name)
        {
            var command = this.commandService.Commands
                .CheckConditions(this.Context, this.serviceProvider)
                .GetAwaiter().GetResult()
                .FirstOrDefault(c => c.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));

            if (command == null)
                return false;

            if (command.IsHidden())
                return false;

            var helpMessage = this.BuildCommandHelpMessageString(command);

            await this.ReplyAsync(helpMessage).ConfigureAwait(false);

            return true;
        }

        private string BuildHelpMessageString(IEnumerable<ModuleHelpInfoModel> moduleInfos)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(string.Format("```Markdown\nAvailable Commands for {0}```", this.Context.Message.Author.Username));

            int i = 0;

            foreach (var mod in moduleInfos)
            {
                i++;

                sb.Append(string.Format("**{0}. {1}** - ", i, mod.Name));

                if (mod.Commands.Any())
                    sb.AppendLine(string.Format("`{0}`", string.Join("`, `", mod.Commands)));

                if (mod.SubModuleNames.Any())
                    sb.AppendLine(string.Format("*SubModules*: **{0}**", string.Join("**, **", mod.SubModuleNames)));

                if (mod.Commands.Any() && mod.SubModuleNames.Any())
                    sb.AppendLine("***Module Not Implemented Yet***");
            }

            sb.AppendLine("```Markdown\nUse 'help <command>' or 'help <module>' for more information\n```");

            return sb.ToString();
        }

        private string BuildModuleHelpMessageString(ModuleInfo module)
        {
            var name = module.Name;
            var summary = module.Summary;
            var aliases = module.Aliases.ToArray().AsEnumerable();
            var moduleCommands = module.Commands.Where(c => !c.IsHidden()).Select(c => c.Name);

            StringBuilder sb = new StringBuilder();

            sb.AppendLine(string.Format("```Markdown\n[{0}]", name));
            sb.AppendLine(new string('=', module.Name.Length + 2));
            sb.AppendLine(string.Format("> {0}\n```", summary));

            if (aliases.Any() && !string.IsNullOrEmpty(aliases.First()))
                sb.AppendLine(string.Format("**Aliases:** `{0}`", string.Join("`, `", aliases)));

            if (moduleCommands.Any())
                sb.AppendLine(string.Format("**Commands:** `{0}`", string.Join("`, `", moduleCommands)));

            return sb.ToString();
        }

        private string BuildCommandHelpMessageString(CommandInfo command)
        {
            var name = command.Name;
            var summary = command.Summary;
            var aliases = command.Aliases.ToArray().AsEnumerable();
            var parameters = command.Parameters;

            StringBuilder sb = new StringBuilder();

            sb.AppendLine(string.Format("```Markdown\n{0}", name));
            sb.AppendLine(new string('=', name.Length + 2));
            sb.AppendLine(string.Format("> {0}\n```", summary));

            if (aliases.Any() && !string.IsNullOrEmpty(aliases.First()))
                sb.AppendLine(string.Format("**Aliases:** `{0}`", string.Join("`, `", aliases)));

            if (parameters.Any())
            {
                var paramNames = parameters.Select(p => p.Name);

                sb.AppendLine(string.Format("[{0}]", string.Join("] [", paramNames)));
                sb.AppendLine();
                sb.AppendLine("__Parameters__");
                foreach (var para in parameters)
                {
                    sb.AppendLine(string.Format("**{0}:** {1}", para.Name, para.Summary));
                }
            }

            return sb.ToString();
        }
    }
}