// <copyright file="App.cs" company="Ryan Blackmore">.
// Copyright © 2020 Ryan Blackmore. All rights Reserved.
// </copyright>

namespace GruggbotBootstrapper
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Threading.Tasks;

    using Gruggbot.Data;
    using Gruggbot.DomainModel;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Test Host Service.
    /// </summary>
    public class App : IHostedService
    {
        private readonly ILogger<App> logger;
        private readonly IHostEnvironment hostEnvironment;
        private readonly IConfiguration configuration;
        private readonly GruggbotContext context;

        public App(ILogger<App> logger, IHostEnvironment hostEnvironment, IConfiguration configuration, IDbContextFactory<GruggbotContext> contextFactory)
        {
            if (contextFactory == null)
                throw new ArgumentNullException(nameof(contextFactory));

            this.logger = logger;
            this.hostEnvironment = hostEnvironment;
            this.configuration = configuration;
            this.context = contextFactory.CreateDbContext();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            this.LogMessage("Begin App Testing");

            Directory.CreateDirectory("Data");

            this.context.Database.EnsureCreated();

            var command = new CountdownCommand
            {
                Name = "ShadowLands",
                Summary = "Displays Countdown to Shadowlands",
                EndDate = new DateTime(2020, 11, 27, 10, 00, 00),
                Event = "World of Warcraft: Shadowlands",
            };

            command.Aliases = new List<CommandAlias>
            {
                new CommandAlias { Alias = "sl" },
                new CommandAlias { Alias = "shadowlambs" },
            };

            command.Messages = new List<CommandMessage>
            {
                new CommandMessage { Sequence = 1, Text = "{event} will be released in {countdown} on {date} at {time}" },
            };

            this.LogMessage("Adding New Command");

            this.context.Commands.Add(command);
            this.context.SaveChanges();

            this.LogMessage("Command Saved");

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            this.logger.LogInformation("Closing Down");
            return Task.CompletedTask;
        }

        private void LogMessage(string message)
        {
            this.logger.LogInformation(message);
        }
    }
}
