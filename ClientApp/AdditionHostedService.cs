using Microsoft.Extensions.Hosting;
using Orleans;
using System;
using System.Threading;
using System.Threading.Tasks;
using TestGrain;

namespace ClientApp
{
    public class AdditionHostedService : IHostedService
    {
        private readonly IHostApplicationLifetime appLife;
        private readonly IClusterClient client;

        public AdditionHostedService(
            IHostApplicationLifetime appLife, IClusterClient client)
        {
            this.appLife = appLife;
            this.client = client;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // since this is an event handler, the lambda's async void is acceptable
            appLife.ApplicationStarted.Register(async () => await ExecuteAsync());
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;

        private async Task ExecuteAsync()
        {
            Console.WriteLine("Addition service started.");
            var adder = client.GetGrain<IAdderGrain>(0);
            Console.WriteLine("Grain reference obtained.");
            var result = await adder.Add(10, 20);
            Console.WriteLine($"Grain invoked: 10 + 20 = {result}");
            Console.WriteLine("Calling StopApplicaton.");

            appLife.StopApplication();
        }
    }
}
