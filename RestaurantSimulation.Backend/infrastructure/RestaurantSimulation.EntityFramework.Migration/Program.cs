using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RestaurantSimulation.EntityFramework.Migration.Services;
using Serilog;

namespace RestaurantSimulation.EntityFramework.Migration
{
    public sealed class Program
    {
        private static CancellationTokenSource _cancellationToken = new CancellationTokenSource();

        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args)
                .Build()
                .RunAsync(_cancellationToken.Token);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((configurationBuilder) =>
                {
                    configurationBuilder
                        .SetBasePath(Environment.CurrentDirectory)
                        .AddJsonFile("appsettings.json", optional: false)
                        .AddEnvironmentVariables();
                })
                .UseSerilog((hostingContext, loggerConfiguration) =>
                {
                    loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<RunMigrationBackgroundService>();
                });
    }
}