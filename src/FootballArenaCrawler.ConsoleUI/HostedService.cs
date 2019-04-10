using FootballArenaCrawler.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Neptuo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
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
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            await client.LoginAsync(configuration.Username, configuration.Password, cancellationToken);

            var playerIdentities = await client.GetPlayersAsync(configuration.TeamId, cancellationToken);
            PrintPlayerIdentities(playerIdentities);

            foreach (PlayerIdentity playerIdentity in playerIdentities)
            {
                var playerDetail = await client.GetPlayerDetailAsync(playerIdentities.First().Id, cancellationToken);
                PrintPlayerDetail(playerDetail);
            }
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
