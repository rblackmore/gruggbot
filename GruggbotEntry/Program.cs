﻿using System;
using System.IO;
using System.Threading.Tasks;

using Gruggbot.Core;
using Gruggbot.Core.Configuration;
using Gruggbot.Core.DependencyInjection;

using GruggbotEntry.LogFilters;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;

using Serilog;
using Serilog.Formatting.Json;

namespace GruggbotEntry
{
    class Program
    {
        private readonly string _configPath = $"data/AppSettings/appsettings.json";
        private readonly string _logBotPath = "data/logs/bot.log";
        private readonly string _logCommandsPath = "data/logs/commands.log";
        private readonly string _imagesPath = $"data/Images";

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
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()

                //PRIMARY LOGGER
                .WriteTo.Logger(lc => lc
                    .Filter.ByExcluding(lev => new CommandLogFilter().IsEnabled(lev))
                    .WriteTo.Debug()
                    .WriteTo.File(_logBotPath, rollingInterval: RollingInterval.Month)
                    .WriteTo.Console())

                //COMMAND LOGGER
                .WriteTo.Logger(lc => lc
                    .Filter.With<CommandLogFilter>()
                    .WriteTo.Debug()
                    .WriteTo.File(new JsonFormatter(), _logCommandsPath, rollingInterval: RollingInterval.Day))

                .CreateLogger();

            services.AddLogging(builder =>
                builder.AddSerilog(dispose: true));

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