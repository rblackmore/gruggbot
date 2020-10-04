﻿// <copyright file="CommandInfoExtensions.cs" company="Ryan Blackmore">.
// Copyright © 2020 Ryan Blackmore. All rights Reserved.
// </copyright>

namespace Gruggbot.Core.DiscordExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Discord.Commands;
    using Gruggbot.Core.CommandModules;

    public static class CommandInfoExtensions
    {
        public static async Task<IEnumerable<CommandInfo>> CheckConditions(this IEnumerable<CommandInfo> commands, ICommandContext ctx, IServiceProvider map)
        {
            var availableCommands = new List<CommandInfo>();

            foreach (var cmd in commands)
            {
                if ((await cmd.CheckPreconditionsAsync(ctx,map).ConfigureAwait(false)).IsSuccess)
                {
                    availableCommands.Add(cmd);
                }
            }

            return availableCommands;
        }

        /// <summary>
        /// Converts list of commands into a string list of distinct command names.
        /// </summary>
        /// <param name="commands">The command list to convert.</param>
        /// <param name="ctx">Context in which the command was called.</param>
        /// <param name="map">Service provider for command system.</param>
        /// <returns>IEnumerable of command name strings.</returns>
        public static async Task<IEnumerable<string>> GetCommandListStringAsync(this IEnumerable<CommandInfo> commands, ICommandContext ctx, IServiceProvider map)
        {
            var cmds =
                from cmd in await commands.CheckConditions(ctx, map).ConfigureAwait(false)
                select cmd.Name;

            return cmds.Distinct();
        }

        internal static bool IsHidden(this CommandInfo command)
        {
            return command.Preconditions.Any(pc => pc is HiddenAttribute);
        }
    }
}
