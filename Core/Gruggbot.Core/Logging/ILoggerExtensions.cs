using Discord.Commands;
using Gruggbot.Core.Logging;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gruggbot.Core.Logging
{
    public static class ILoggerExtensions
    {
        internal static void LogCommandCall<T>(this ILogger<T> logger, string authorName, string commandName, params string[] paras) where T : ModuleBase
        {

            var eventLog = new CommandEventLog
            {
                Author = authorName,
                Command = commandName,
                DateTime = DateTime.Now,
                Module = typeof(T).Name,
                Params = paras
            };

            logger.LogInformation("Command Executed: {@Command}", eventLog);
        }
    }
}
