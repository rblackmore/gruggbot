// <copyright file="App.cs" company="Ryan Blackmore">.
// Copyright © 2020 Ryan Blackmore. All rights Reserved.
// </copyright>

namespace GruggbotBootstrapper
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
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

        public App(ILogger<App> logger, IHostEnvironment hostEnvironment, IConfiguration configuration)
        {
            this.logger = logger;
            this.hostEnvironment = hostEnvironment;
            this.configuration = configuration;

        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            this.logger.LogInformation("Environment: {0}", this.hostEnvironment.EnvironmentName);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            this.logger.LogInformation("Closing Down");
            return Task.CompletedTask;
        }
    }
}
