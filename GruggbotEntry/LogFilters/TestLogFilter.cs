using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace GruggbotEntry.LogFilters
{
    class TestLogFilter : ILogEventFilter
    {
        public bool IsEnabled(LogEvent logEvent)
        {
            var vent = logEvent;


            foreach (var prop in logEvent.Properties)
            {
                Console.WriteLine($"Filter: {prop.Key}:{prop.Value}");
            }

            return true;
        }
    }
}
