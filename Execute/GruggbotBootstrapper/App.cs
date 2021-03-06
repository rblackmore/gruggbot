﻿// <copyright file="App.cs" company="Ryan Blackmore">.
// Copyright © 2020 Ryan Blackmore. All rights Reserved.
// </copyright>

namespace GruggbotBootstrapper
{
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

            var info = this.hostEnvironment.ContentRootFileProvider.GetFileInfo("/Content/Images/classic.png");

            if (!info.Exists)
                this.logger.LogError("File '{file}' does not exist", info.PhysicalPath);
            else
                this.logger.LogInformation("File '{file}' exists", info.PhysicalPath);

            this.logger.LogInformation("Serilog Sink 0: {0}", this.configuration.GetSection("Serilog:WriteTo:0:Name").Value);
            this.logger.LogInformation("Serilog Sink 1: {0}", this.configuration.GetSection("Serilog:WriteTo:1:Name").Value);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            this.logger.LogInformation("Closing Down");
            return Task.CompletedTask;
        }
    }
}
