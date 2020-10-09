// <copyright file="HelpModule.cs" company="Ryan Blackmore">.
// Copyright © 2020 Ryan Blackmore. All rights Reserved.
// </copyright>

namespace Gruggbot.CommandModules
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Discord.Commands;
    using Gruggbot.CommandModules.Model;
    using Gruggbot.Extensions;
    using Microsoft.Extensions.Logging;

    [Summary("Helpful Information about the commands available")]
    public class HelpModule : ModuleBase
    {
#pragma warning disable IDE0052 // Remove unread private members
        private readonly ILogger<HelpModule> logger;
#pragma warning restore IDE0052 // Remove unread private members
        private readonly CommandService commandService;
        private readonly IServiceProvider serviceProvider;

        public HelpModule(ILogger<HelpModule> logger, CommandService commandService, IServiceProvider serviceProvider)
        {
            this.logger = logger;
            this.commandService = commandService;
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

            var helpMessage = HelpMessageBuilder.BuildHelpMessageString(modules, this.Context.User.Username);

            await this.ReplyAsync(helpMessage).ConfigureAwait(false);
        }

        /// <summary>
        /// Displays a help message on a specified command or module.
        /// </summary>
        /// <param name="name">Name of command or module.</param>
        /// <returns>An awaitable Task.</returns>
        [Command("help")]
        [Summary("Displays a very helpful message about a Command or Command Module")]
        public async Task Help([Summary("The Command or Command Module to get help for")] string name)
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
        public async Task Help(
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

            var helpMessage = HelpMessageBuilder.BuildModuleHelpMessageString(module);

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

            var helpMessage = HelpMessageBuilder.BuildCommandHelpMessageString(command);

            await this.ReplyAsync(helpMessage).ConfigureAwait(false);

            return true;
        }
    }
}