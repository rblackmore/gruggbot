// <copyright file="BotConfiguration.cs" company="Ryan Blackmore">.
// Copyright © 2020 Ryan Blackmore. All rights Reserved.
// </copyright>

namespace Gruggbot.Configuration
{
    public class BotConfiguration
    {
        public const string Bot = "Bot";

        public string Token { get; set; }

        public CommandHandlerConfiguration Commands { get; set; }

    }
}
