using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;

namespace Gruggbot.Core.DiscordExtensions
{
    public static class ModuleInfoExtensions
    {
        /// <summary>
        /// Checks Module for any executable commands by the calling User
        /// </summary>
        /// <param name="module">Module to check</param>
        /// <param name="ctx">Context in which the command was called</param>
        /// <param name="map">Service Provider for command system</param>
        /// <returns>Boolean value indicating if the user can execute commands in this module</returns>
        public static async Task<bool> CheckConditions(this ModuleInfo module, ICommandContext ctx, IServiceProvider map = null)
        {
            bool canExecute = false;

            var cmds = await module.Commands.CheckConditions(ctx, map).ConfigureAwait(false);
            var subs = await module.Submodules.CheckConditions(ctx, map);

            if (cmds.Any() || subs.Any())
                canExecute = true;

            return canExecute;
        }
        /// <summary>
        /// Checks Module list for any executable commands by the calling user.
        /// </summary>
        /// <param name="modules">Module list to check</param>
        /// <param name="ctx">Context in which the command was called</param>
        /// <param name="map">Service Provide for the command system</param>
        /// <returns>Enumerable of all modules the user can execute commands within</returns>
        /// <remarks>Recursively calls itself for each submodule list</remarks>
        public static async Task<IEnumerable<ModuleInfo>> CheckConditions(this IEnumerable<ModuleInfo> modules, ICommandContext ctx, IServiceProvider map = null)
        {
            var availableModules = new List<ModuleInfo>();

            foreach(var mod in modules)
            {
                var subs = await mod.Submodules.CheckConditions(ctx, map).ConfigureAwait(false);
                var cmds = await mod.Commands.CheckConditions(ctx, map).ConfigureAwait(false);

                if (subs.Any() || cmds.Any())
                    availableModules.Add(mod);
            }

            return availableModules;
        }
    }
}
