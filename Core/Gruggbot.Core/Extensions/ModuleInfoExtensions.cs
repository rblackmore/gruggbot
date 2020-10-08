// <copyright file="ModuleInfoExtensions.cs" company="Ryan Blackmore">.
// Copyright © 2020 Ryan Blackmore. All rights Reserved.
// </copyright>

namespace Gruggbot.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Discord.Commands;
    using Gruggbot.CommandModules;

    public static class ModuleInfoExtensions
    {
        /// <summary>
        /// Checks Module for any executable commands by the calling User.
        /// </summary>
        /// <param name="module">Module to check</param>
        /// <param name="context">Context in which the command was called</param>
        /// <param name="services">Service Provider for command system</param>
        /// <returns>Boolean value indicating if the user can execute commands in this module</returns>
        internal static async Task<bool> CheckConditions(
            this ModuleInfo module,
            ICommandContext context,
            IServiceProvider services = null)
        {
            bool canExecute = false;

            var cmds = await module.Commands.CheckConditions(context, services).ConfigureAwait(false);
            var subs = await module.Submodules.CheckConditions(context, services).ConfigureAwait(false);

            if (cmds.Any() || subs.Any())
                canExecute = true;

            return canExecute;
        }

        /// <summary>
        /// Checks Module list for any executable commands by the calling user.
        /// </summary>
        /// <param name="modules">Module list to check</param>
        /// <param name="context">Context in which the command was called.</param>
        /// <param name="services">Service Provide for the command system.</param>
        /// <returns>Enumerable of all modules the user can execute commands within.</returns>
        /// <remarks>Recursively calls itself for each submodule list.</remarks>
        internal static async Task<IEnumerable<ModuleInfo>> CheckConditions(
            this IEnumerable<ModuleInfo> modules,
            ICommandContext context,
            IServiceProvider services = null)
        {
            var availableModules = new List<ModuleInfo>();

            foreach (var mod in modules)
            {
                var subs = await mod.Submodules.CheckConditions(context, services).ConfigureAwait(false);
                var cmds = await mod.Commands.CheckConditions(context, services).ConfigureAwait(false);

                if (subs.Any() || cmds.Any())
                    availableModules.Add(mod);
            }

            return availableModules;
        }

        internal static bool IsHidden(this ModuleInfo module)
        {
            return module.Preconditions.Any(pc => pc is HiddenAttribute);
        }
    }
}
