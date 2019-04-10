using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FootballArenaCrawler
{
    internal class HostedService : IHostedService
    {
        private async Task RunAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(500);
            Console.WriteLine("Hello, World!");
            await Task.Delay(500);
            Environment.Exit(0);
        }

        #region IHostedService

        private CancellationTokenSource cancellationTokenSource;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _ = RunAsync(cancellationTokenSource.Token);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            cancellationTokenSource.Cancel();
            return Task.CompletedTask;
        } 

        #endregion
    }
}
