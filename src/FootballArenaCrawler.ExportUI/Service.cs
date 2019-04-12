using FootballArenaCrawler.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Neptuo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FootballArenaCrawler
{
    internal class Service : IHostedService
    {
        private readonly ApiClient client;
        private readonly ILogger log;
        private readonly Configuration configuration;

        public Service(ApiClient client, ILogger<Service> log, IOptions<Configuration> configuration)
        {
            Ensure.NotNull(client, "client");
            Ensure.NotNull(log, "log");
            Ensure.NotNull(configuration, "configuration");
            this.client = client;
            this.log = log;
            this.configuration = configuration.Value;
        }

        private void ValidateConfiguration()
        {
            bool isError = false;
            void SetError(bool error, string messsage)
            {
                if (error)
                {
                    log.LogCritical(messsage);
                    isError = true;
                }
            }

            SetError(String.IsNullOrEmpty(configuration.Username), "Missing Username");
            SetError(String.IsNullOrEmpty(configuration.Password), "Missing Password");
            SetError(configuration.TeamId <= 0, "Missing TeamId");
            SetError(String.IsNullOrEmpty(configuration.ExportPath), "Missing ExportPath");

            string directoryPath = Path.GetDirectoryName(configuration.ExportPath);
            if (!String.IsNullOrEmpty(directoryPath))
                SetError(!Directory.Exists(directoryPath), "Missing ExportPath directory doesn't exist");

            if (isError)
                throw Ensure.Exception.InvalidOperation("Configuration validation failed.");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            ValidateConfiguration();

            log.LogInformation($"Exporting TeamId '{configuration.TeamId}'.");

            Export export = new Export();

            await client.LoginAsync(configuration.Username, configuration.Password, cancellationToken);

            export.SeasonNumber = await client.GetSeasonNumberAsync(cancellationToken);
            log.LogInformation($"Exporting season number '{export.SeasonNumber}'.");

            var playerIdentities = await client.GetPlayersAsync(configuration.TeamId, cancellationToken);
            log.LogInformation($"Found '{playerIdentities.Count}' player identities.");

            foreach (PlayerIdentity playerIdentity in playerIdentities)
            {
                log.LogInformation($"Exporting player '{playerIdentity.Name}'.");
                var playerDetail = await client.GetPlayerDetailAsync(playerIdentity.Id, cancellationToken);
                export.Players.Add(playerDetail);
            }

            ExportJson(export);
            Environment.Exit(0);
        }

        private void ExportJson(Export export)
        {
            log.LogInformation($"Saving export to '{configuration.ExportPath}'.");
            string json = JsonConvert.SerializeObject(export, Formatting.Indented);
            File.WriteAllText(configuration.ExportPath, json);
        }

        #region IHostedService

        private CancellationTokenSource cancellationTokenSource;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            RunOverrideAsync(cancellationTokenSource.Token).ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    log.LogCritical(t.Exception.InnerException, "Application failed.");
                    Environment.Exit(1);
                }
            });

            return Task.CompletedTask;
        }

        private async Task RunOverrideAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(100);
            await RunAsync(cancellationTokenSource.Token);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            cancellationTokenSource.Cancel();
            return Task.CompletedTask;
        }

        #endregion
    }
}
