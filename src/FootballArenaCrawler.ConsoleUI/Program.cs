﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Neptuo;
using Neptuo.Converters;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace FootballArenaCrawler
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await new HostBuilder()
               .UseEnvironment("Development")
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
                .AddJsonFile($"AppSettings.{context.HostingEnvironment.EnvironmentName}.json");
        }

        private static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            Converts.Repository
                .AddStringTo<int>(Int32.TryParse)
                .AddStringTo<bool>(Boolean.TryParse)
                .AddStringTo<decimal>(Decimal.TryParse)
                .AddStringTo<double>(Double.TryParse)
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
            //services.AddHttpClient<ApiClient>(httpClient =>
            //{
            //    httpClient.BaseAddress = new Uri("https://www.footballarena.org/");
            //});
            services.AddHostedService<HostedService>();
        }
    }
}
