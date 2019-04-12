using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Neptuo;
using Neptuo.Converters;
using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;

namespace FootballArenaCrawler
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await new HostBuilder()
#if DEBUG
               .UseEnvironment("Development")
#else
               .UseEnvironment("Production")
#endif
               .ConfigureLogging(builder => builder.AddConsole())
               .ConfigureAppConfiguration(ConfigureAppConfiguration)
               .ConfigureServices(ConfigureServices)
               .UseConsoleLifetime()
               .RunConsoleAsync();
        }

        private static void ConfigureAppConfiguration(HostBuilderContext context, IConfigurationBuilder builder)
        {
            builder
                .AddJsonFile("AppSettings.json", true)
                .AddJsonFile($"AppSettings.{context.HostingEnvironment.EnvironmentName}.json", true);
        }

        private static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            Converts.Repository
                .AddStringTo<int>(Int32.TryParse)
                .AddStringTo<bool>(Boolean.TryParse)
                .AddStringTo<decimal>(Decimal.TryParse)
                .AddStringTo<double>(TryParseDouble)
                .AddEnumSearchHandler(false);

            services.Configure<Configuration>(context.Configuration);
            services.AddTransient(provider =>
            {
                HttpClientHandler httpHandler = new HttpClientHandler();
                httpHandler.AllowAutoRedirect = false;

                HttpClient httpClient = new HttpClient(httpHandler);
                httpClient.BaseAddress = new Uri("https://www.footballarena.org/");

                return new ApiClient(httpClient, new HtmlParser());
            });
            services.AddHostedService<Service>();
        }

        private static bool TryParseDouble(string input, out double output) => Double.TryParse(input, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out output);
    }
}
