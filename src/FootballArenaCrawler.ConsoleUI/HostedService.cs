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
        }

        private void PrintPlayerIdentities(IReadOnlyCollection<PlayerIdentity> playerIdentities)
        {
            foreach (PlayerIdentity playerIdentity in playerIdentities)
                Console.WriteLine($"{String.Format("{0,10}", playerIdentity.Id)} {playerIdentity.Name}");
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
