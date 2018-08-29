using System;
using System.Collections.Generic;
using System.Text;

namespace Gruggbot.Core.Logging
{
    public struct CommandEventLog
    {
        public DateTime DateTime { get; set; }
        public string Author { get; set; }
        public string Module { get; set; }
        public string Command { get; set; }
        public string[] Params { get; set; }
    }
}
