using Discord;
using Gruggbot.Core;
using Gruggbot.Core.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Gruggbot.Core.Configuration;

namespace GruggbotEntry
{
    class Program
    {
        private readonly string _configPath = $"data/AppSettings/appsettings.json";

        private IServiceProvider _serviceProvider;
        private IConfigurationRoot _configurationRoot;

        static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            ServiceCollection services = new ServiceCollection();
            bool success = await ConfigureServices(services);

            if (!success)
            {
                return;
            }

            _serviceProvider = services.BuildServiceProvider();

            _configurationRoot = _serviceProvider.GetService<IConfigurationRoot>();

            await _serviceProvider.GetService<BotApp>().Run();

            Console.ReadKey();
        }

        private async Task<bool> ConfigureServices(IServiceCollection services)
        {
            if (!File.Exists(_configPath))
            {
                new FileInfo(_configPath).Directory.Create();
                File.WriteAllText(_configPath, JsonConvert.SerializeObject(new AppSettings(), Formatting.Indented));
                return false;
            }

            services.AddSingleton<IConfigurationRoot>(await GetConfiguration());

            //Configure Serilog


            //Configure Microsoft Logging
            services.AddSingleton(new LoggerFactory()
                .AddConsole()
                .AddDebug());

            services.AddLogging();

            services.AddBot();

            return true;

        }

        private Task<IConfigurationRoot> GetConfiguration()
        {
            IConfigurationRoot configurationRoot = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddEnvironmentVariables()
                .AddJsonFile(_configPath, optional: true, reloadOnChange: true)
                .Build();

            return Task.FromResult(configurationRoot);
        }

    }
}