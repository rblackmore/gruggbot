// <copyright file="Program.cs" company="Ryan Blackmore">.
// Copyright © 2020 Ryan Blackmore. All rights Reserved.
// </copyright>

namespace GruggbotBootstrapper;

using System;
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
      .ConfigureHostConfiguration(ConfigureConfigurationBuilder)
      .ConfigureServices((context, services) =>
      {
        // services.AddHostedService<App>();
        services.AddBot(context.Configuration);
      })
      .UseSerilog((context, loggerConfiguration) =>
      {
        loggerConfiguration.ReadFrom.Configuration(context.Configuration);
      });

  /// <summary>
  /// Configures the Configuration Builder.
  /// </summary>
  /// <returns>Configuration Builder.</returns>
  public static void ConfigureConfigurationBuilder(IConfigurationBuilder builder)
  {
    var envSettings =
      $"appsettings.{Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? Environments.Production}.json";

    builder.SetBasePath($"{AppDomain.CurrentDomain.BaseDirectory}/config")
      .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
      .AddJsonFile(envSettings, optional: true)
      .AddEnvironmentVariables();
  }

  /// <summary>
  /// Configures Serilog for use before the Host is built.
  /// </summary>
  public static void ConfigureLogging()
  {
    var builder = new ConfigurationBuilder();
    ConfigureConfigurationBuilder(builder);
    var config = builder.Build();

    Log.Logger = new LoggerConfiguration()
      .ReadFrom.Configuration(config)
      .CreateLogger();
  }
}
