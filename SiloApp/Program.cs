using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.Net;
using System.Threading.Tasks;

using TestGrain;

namespace SiloApp
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                var host = Host.CreateDefaultBuilder(args);

                host.UseOrleans(builder =>
                {
                    builder
                    .UseLocalhostClustering()
                    .Configure<EndpointOptions>(options => options.AdvertisedIPAddress = IPAddress.Loopback)
                    .ConfigureApplicationParts(parts =>
                    {
                        parts.AddApplicationPart(typeof(AdderGrain).Assembly).WithReferences();
                    });
                });

                host.ConfigureLogging(builder =>
                {
                    builder
                    .AddFilter("Microsoft", LogLevel.Warning)   // generic host lifecycle messages
                    .AddFilter("Orleans", LogLevel.Warning)     // suppress status dumps
                    .AddFilter("Runtime", LogLevel.Warning)     // also an Orleans prefix
                    .AddDebug()                                 // VS Debug window
                    .AddConsole();
                });

                host.ConfigureServices(services =>
                {
                    services.AddLogging();
                });

                await host.RunConsoleAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}