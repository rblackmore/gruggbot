using System;
using System.Collections.Generic;
using System.Text;

namespace Gruggbot.Core.Configuration
{
    public class BotConfiguration
    {
        public const string Bot = "Bot";
        public string Token { get; set; }
        public char Prefix { get; set; }
    }
}
