using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using Serilog;

namespace GruggbotBootstrapper.Logging
{
    public static class LoggingSetup
    {
        public static void ConfigureLogging(string environment)
        {
            var builder = new ConfigurationBuilder();
            BuildConfig(builder, environment);

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Build())
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();
        }

        public static void BuildConfig(IConfigurationBuilder builder, string environment)
        {
            string environmentSettings = $"appsettings.{environment}.json";

            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile(environmentSettings, optional: true)
                .AddEnvironmentVariables();
        }
    }
}
