// <copyright file="RandomMessagesConfiguration.cs" company="Ryan Blackmore">.
// Copyright © 2020 Ryan Blackmore. All rights Reserved.
// </copyright>

namespace Gruggbot.Configuration
{
    /// <summary>
    /// Configuration options for <see cref="RandomMessages"/>.
    /// </summary>
    public class RandomMessagesConfiguration
    {
        public const string RandomMessages = "RandomMessages";

        public int DefaultChance { get; set; }

        public int ChanceIncrement { get; set; }
    }
}
