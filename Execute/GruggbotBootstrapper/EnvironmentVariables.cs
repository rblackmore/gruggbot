// <copyright file="EnvironmentVariables.cs" company="Ryan Blackmore">.
// Copyright © 2020 Ryan Blackmore. All rights Reserved.
// </copyright>

namespace GruggbotBootstrapper
{
    using System;

    using Microsoft.Extensions.Hosting;

    public static class EnvironmentVariables
    {
        /// <summary>
        /// Gets value indicating what mode the app is in. Defaults to 'Bot'.
        /// </summary>
        public static string Mode => Environment.GetEnvironmentVariable("APP_MODE") ?? "Bot";

        /// <summary>
        /// Gets value indicating the current environment. Defaults to 'Production'.
        /// </summary>
        public static string Env => Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? Environments.Production;

        /// <summary>
        /// Gets a value indicating whether the current mode is 'Test'.
        /// </summary>
        public static bool IsTestMode => string.Equals(Mode, "Test", StringComparison.InvariantCultureIgnoreCase);
    }
}
