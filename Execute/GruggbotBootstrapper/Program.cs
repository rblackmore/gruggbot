// <copyright file="Program.cs" company="Ryan Blackmore">.
// Copyright © 2020 Ryan Blackmore. All rights Reserved.
// </copyright>

namespace GruggbotBootstrapper
{
    using System;
    using System.Threading.Tasks;

    using Gruggbot.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Serilog;
    using Serilog.Configuration;

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