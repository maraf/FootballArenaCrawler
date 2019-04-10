using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FootballArenaCrawler
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await new HostBuilder()
               .ConfigureLogging(builder => builder.AddConsole())
               .ConfigureServices(ConfigureServices)
               .UseConsoleLifetime()
               .RunConsoleAsync();
        }

        static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            services.AddHostedService<HostedService>();
        }
    }
}
