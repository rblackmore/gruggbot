using System;
using System.Threading.Tasks;

using Gruggbot.Core.DependencyInjection;

using GruggbotBootstrapper.Logging;

using Microsoft.Extensions.Hosting;

using Serilog;

namespace GruggbotBootstrapper
{
    class Program
    {
        static async Task Main(string[] args)
        {
            LoggingSetup.ConfigureLogging(CurrentEnvironment);

            try
            {
                await CreateHostBuilder(args).Build().RunAsync();
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseEnvironment(CurrentEnvironment)
                .ConfigureServices((context, services) =>
                {
                    services.AddBot(context.Configuration);
                })
                .UseSerilog();

        public static string CurrentEnvironment
        {
            get =>
                Environment.GetEnvironmentVariable("CONSOLENETCORE_ENVIRONMENT") ?? Environments.Production;
        }
    }
}
