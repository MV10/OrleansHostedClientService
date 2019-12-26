using Microsoft.Extensions.Hosting;
using Orleans;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ClientApp
{
    public class ClusterClientHostedService : IHostedService
    {
        private readonly IClusterClient client;

        public ClusterClientHostedService(IClusterClient client)
        {
            this.client = client;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Cluster client service started.");
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Cluster client service stopping.");
            await client?.Close();
            client?.Dispose();
        }
    }
}
