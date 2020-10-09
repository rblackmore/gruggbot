﻿// <copyright file="TestingModule.cs" company="Ryan Blackmore">.
// Copyright © 2020 Ryan Blackmore. All rights Reserved.
// </copyright>

namespace Gruggbot.CommandModules
{
    using System.Threading.Tasks;

    using Discord.Commands;
    using Microsoft.Extensions.Logging;

    public enum TestOption
    {
        None,
        Dull,
        Fancy,
        Extreme,
    }

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

        [Command("multi")]
        public async Task MultiOptionalParams(string name, string type = null)
        {
            if (type == null)
                this.logger.LogInformation("Type is null");

            this.logger.LogInformation("Name: {name} - Type: {type}", name, type);
            await this.ReplyAsync(string.Format("Name: {0} - Type: {1}", name, type)).ConfigureAwait(false);
        }

        [Command("reader")]
        public async Task ReaderTest(TestOption selection = TestOption.None)
        {
            this.logger.LogTrace("Option selected is `{option}`", selection);
            await this.ReplyAsync(string.Format("Selected Option: `{0}`", selection)).ConfigureAwait(false);
        }
    }
}
