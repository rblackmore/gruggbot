using Discord;
using Gruggbot.Core;
using Gruggbot.Core.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ConsoleTest
{
    class Program
    {

        private IServiceProvider _serviceProvider;
        private IConfigurationRoot _configurationRoot;

        static void Main(string[] args) 
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            ServiceCollection services = new ServiceCollection();
            await ConfigureServices(services);

            _serviceProvider = services.BuildServiceProvider();

            _configurationRoot = _serviceProvider.GetService<IConfigurationRoot>();

            await _serviceProvider.GetService<BotApp>().Run();

            Console.ReadKey();
        }

        private Task ConfigureServices(IServiceCollection services)            
        {

            IConfigurationRoot configurationRoot = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddEnvironmentVariables()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            services.AddSingleton<IConfigurationRoot>(configurationRoot);
            services.AddSingleton(new LoggerFactory()
                .AddConsole()
                .AddDebug());
                
            services.AddLogging();

            services.AddBot();

            return Task.CompletedTask;
        }
    }
}
