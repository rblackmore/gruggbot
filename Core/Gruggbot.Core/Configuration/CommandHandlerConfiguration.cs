// <copyright file="CommandHandlerConfiguration.cs" company="Ryan Blackmore">.
// Copyright © 2020 Ryan Blackmore. All rights Reserved.
// </copyright>

namespace Gruggbot.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class CommandHandlerConfiguration
    {
        public const string Commands = "Bot:Commands";
        public const string CommandServiceConfig = "Bot:Commands:Config";

        public char Prefix { get; set; }
    }
}
