// <copyright file="HelpModule.MessageBuilders.cs" company="Ryan Blackmore">.
// Copyright © 2020 Ryan Blackmore. All rights Reserved.
// </copyright>

namespace Gruggbot.CommandModules
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Discord.Commands;
    using Gruggbot.CommandModules.Model;
    using Gruggbot.Extensions;

    internal static class HelpMessageBuilder
    {
        public static string BuildModuleHelpMessageString(ModuleInfo module)
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

        public static string BuildCommandHelpMessageString(CommandInfo command)
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

        public static string BuildHelpMessageString(IEnumerable<ModuleHelpInfoModel> moduleInfos, string userName)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(string.Format("```Markdown\nAvailable Commands for {0}```", userName));

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

    }
}
