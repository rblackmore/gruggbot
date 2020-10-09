// <copyright file="ModuleHelpInfoModel.cs" company="Ryan Blackmore">.
// Copyright © 2020 Ryan Blackmore. All rights Reserved.
// </copyright>

namespace Gruggbot.CommandModules.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Discord.Commands;
    using Gruggbot.Extensions;

    public class ModuleHelpInfoModel
    {
        public string Name { get; set; }

        public string Summary { get; set; }

        public IEnumerable<string> SubModuleNames { get; set; }

        public IReadOnlyList<ModuleInfo> SubModules { get; set; }

        public int AliasCount { get; set; }

        public IEnumerable<string> Aliases { get; set; }

        public IEnumerable<string> Commands { get; set; }

        internal static ModuleHelpInfoModel CreateFromModuleInfo(
            ModuleInfo modInfo,
            ICommandContext context,
            IServiceProvider services)
        {
            return new ModuleHelpInfoModel
            {
                Name = modInfo.Name,
                Summary = modInfo.Summary,
                SubModuleNames = modInfo.Submodules.Select(sm => sm.Name).AsEnumerable(),
                SubModules = modInfo.Submodules,
                Aliases = modInfo.Aliases.ToArray(),
                Commands = modInfo.Commands.Where(c => !c.IsHidden())
                    .GetCommandListStringAsync(context, services).Result.AsEnumerable(),
            };
        }
    }
}
