// <copyright file="Program.cs" company="Ryan Blackmore">.
// Copyright © 2020 Ryan Blackmore. All rights Reserved.
// </copyright>

namespace GruggbotBootstrapper
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    using Gruggbot.DependencyInjection;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Serilog;

    public static class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                ConfigureLogging();

                await CreateHostBuilder(args).Build().RunAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.Logger.Fatal(ex.Message);
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

        public static IConfigurationBuilder CreateConfigurationBuilder()
        {
            var builder = new ConfigurationBuilder();

            var env_settings =
                $"appsettings.{Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? Environments.Production}.json";

            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile(env_settings, optional: true)
                .AddEnvironmentVariables();

            return builder;
        }

        public static void ConfigureLogging()
        {
            var config = CreateConfigurationBuilder().Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .CreateLogger();
        }
    }
}