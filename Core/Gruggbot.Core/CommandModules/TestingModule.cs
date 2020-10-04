// <copyright file="TestingModule.cs" company="Ryan Blackmore">.
// Copyright © 2020 Ryan Blackmore. All rights Reserved.
// </copyright>

namespace Gruggbot.Core.CommandModules
{
    using System.Threading.Tasks;

    using Discord.Commands;
    using Microsoft.Extensions.Logging;

    [Summary("Module for running tests during runtime.")]
    [RequireOwner]
    public class TestingModule : ModuleBase
    {
        private readonly ILogger<TestingModule> logger;

        public TestingModule(ILogger<TestingModule> logger)
        {
            this.logger = logger;
        }

        [Command("log")]
        [Summary("Logs an information message with the current logging framework.")]
        public async Task LogMessage(string message)
        {
            this.logger.LogInformation("Message logged: {message}", message);
            await this.ReplyAsync($"Message '{message}' has been logged").ConfigureAwait(false);
        }
    }
}
