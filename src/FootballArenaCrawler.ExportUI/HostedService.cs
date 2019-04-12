using FootballArenaCrawler.Models;
using Microsoft.Extensions.Hosting;
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
    internal class HostedService : IHostedService
    {
        private readonly ApiClient client;
        private readonly Configuration configuration;

        public HostedService(ApiClient client, IOptions<Configuration> configuration)
        {
            Ensure.NotNull(client, "client");
            Ensure.NotNull(configuration, "configuration");
            this.client = client;
            this.configuration = configuration.Value;

            ValidateConfiguration();
        }

        private void ValidateConfiguration()
        {
            bool isError = false;
            void SetError(bool error, string messsage)
            {
                if (error)
                {
                    Console.WriteLine(messsage);
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
            Export export = new Export();

            await client.LoginAsync(configuration.Username, configuration.Password, cancellationToken);

            export.SeasonNumber = await client.GetSeasonNumberAsync(cancellationToken);

            var playerIdentities = await client.GetPlayersAsync(configuration.TeamId, cancellationToken);

            foreach (PlayerIdentity playerIdentity in playerIdentities)
            {
                var playerDetail = await client.GetPlayerDetailAsync(playerIdentity.Id, cancellationToken);
                export.Players.Add(playerDetail);
            }

            ExportJson(export);
            Environment.Exit(0);
        }

        private void ExportJson(Export export)
        {
            string json = JsonConvert.SerializeObject(export, Formatting.Indented);
            File.WriteAllText(configuration.ExportPath, json);
        }

        private void PrintPlayerIdentities(IReadOnlyCollection<PlayerIdentity> playerIdentities)
        {
            foreach (PlayerIdentity playerIdentity in playerIdentities)
                Console.WriteLine($"{String.Format("{0,10}", playerIdentity.Id)} {playerIdentity.Name}");
        }

        private void PrintPlayerDetail(PlayerDetail playerDetail)
        {
            void Write(string title, object value) => Console.WriteLine($"{String.Format("{0,12}", title)}: {value}");

            Write("Id", playerDetail.Id);
            Write("Name", playerDetail.Name);
            Write("Nationality", playerDetail.Nationality);
            Write("Age", playerDetail.Age);
            Write("Position", playerDetail.Position);
            Write("Height", playerDetail.Height);
            Write("Price", playerDetail.Price);
            Write("Salary", playerDetail.Salary);
            Write("SignedAt", playerDetail.SignedAt.ToShortDateString());
            Write("Potential", playerDetail.Potential);
            Write("IsHome", playerDetail.IsHome);
        }

        #region IHostedService

        private CancellationTokenSource cancellationTokenSource;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(500);

            cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _ = RunAsync(cancellationTokenSource.Token);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            cancellationTokenSource.Cancel();
            return Task.CompletedTask;
        }

        #endregion
    }
}
