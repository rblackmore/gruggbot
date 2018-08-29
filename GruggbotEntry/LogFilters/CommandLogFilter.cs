using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace GruggbotEntry.LogFilters
{
    class CommandLogFilter : ILogEventFilter
    {
        public bool IsEnabled(LogEvent logEvent)
        {
            if (logEvent.Properties.TryGetValue("Command", out LogEventPropertyValue p) && p is StructureValue structured)
            {
                return true;
            }

            return false;
        }
    }
}
