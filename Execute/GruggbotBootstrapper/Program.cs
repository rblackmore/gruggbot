// <copyright file="Program.cs" company="Ryan Blackmore">.
// Copyright © 2020 Ryan Blackmore. All rights Reserved.
// </copyright>

#pragma warning disable SA1600 // Elements should be documented

namespace GruggbotBootstrapper
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    using Gruggbot.Core.DependencyInjection;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Serilog;

    public static class Program
    {
        public static async Task Main(string[] args)
        {
            IHost host = null;

            try
            {
                host = CreateHostBuilder(args).Build();

                await host.RunAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.Logger.Fatal(ex.Message);
            }
            finally
            {
                if (host != null)
                    await host.StopAsync().ConfigureAwait(false);
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    // services.AddHostedService<App>();
                    services.AddBot(context.Configuration);
                })
                .UseSerilog((context, loggerConfiguration) =>
                {
                    loggerConfiguration.ReadFrom.Configuration(context.Configuration);
                });
    }
}