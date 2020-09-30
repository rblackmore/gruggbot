using System.Diagnostics.Eventing.Reader;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GruggbotBootstrapper
{
    public class App : IHostedService
    {
        private readonly ILogger<App> logger;
        private readonly IConfiguration configuration;

        public App(ILogger<App> logger, IConfiguration configuration)
        {
            this.logger = logger;
            this.configuration = configuration;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            this.logger.LogInformation("Hello, World");
            this.logger.LogInformation($"Token: {this.configuration.GetSection("Token").Value}");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            this.logger.LogInformation("Closing Down");
            return Task.CompletedTask;
        }
    }
}
