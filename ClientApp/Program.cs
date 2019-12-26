using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ClientApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                var hostBuilder = Host.CreateDefaultBuilder(args);

                Console.WriteLine("Main: AddOrleansClusterClient");
                await hostBuilder.AddOrleansClusterClientAsync();

                hostBuilder.ConfigureServices(services =>
                {
                    Console.WriteLine("Main: Add ClusterClientHostedService");
                    services.AddHostedService<ClusterClientHostedService>();

                    Console.WriteLine("Main: Add AdditionHostedService");
                    services.AddHostedService<AdditionHostedService>();
                });

                Console.WriteLine("Main: RunConsoleAsync");
                await hostBuilder.RunConsoleAsync();
            }
            catch(Exception ex)
            {
                Console.WriteLine($"\n\nException:\n{ex}");
            }

            if(!Debugger.IsAttached)
            {
                Console.WriteLine("\n\nPress any key to exit.");
                Console.ReadKey(true);
            }
        }
    }
}
