using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RestaurantSimulation.Infrastructure.Persistence;

namespace RestaurantSimulation.EntityFramework.Migration.Services
{
    public sealed class RunMigrationBackgroundService : BackgroundService
    {
        private readonly ILogger<RunMigrationBackgroundService> _logger;
        private readonly ILoggerFactory _loggerFactory;
        private readonly IConfiguration _configuration;

        public RunMigrationBackgroundService(
            ILogger<RunMigrationBackgroundService> logger,
            ILoggerFactory loggerFactory,
            IConfiguration configuration)
        {
            _logger = logger;
            _loggerFactory = loggerFactory;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting to execute migrations for the RestaurantSimulation Database");

            while(true)
            {
                try
                {
                    var mySqlConnectionString = _configuration.GetConnectionString("Mysql");

                    if(mySqlConnectionString is null)
                    {
                        throw new ArgumentNullException("Mysql connection string is not defined");
                    }

                    var dbContextOptions = new DbContextOptionsBuilder<RestaurantSimulationContext>()
                        .UseLoggerFactory(_loggerFactory)
                        .EnableSensitiveDataLogging()
                        .UseMySql(mySqlConnectionString,
                            ServerVersion.AutoDetect(mySqlConnectionString),
                            mySqlOptions =>
                            {
                                mySqlOptions.EnableRetryOnFailure(
                                    maxRetryCount: 3,
                                    maxRetryDelay: TimeSpan.FromSeconds(30),
                                    errorNumbersToAdd: null);

                                mySqlOptions.MigrationsAssembly(typeof(RestaurantSimulationContext).Assembly.FullName);
                            });

                    var dbContext = new RestaurantSimulationContext(dbContextOptions.Options);


                    await dbContext.Database.MigrateAsync();

                    break;
                }
                catch(Exception ex)
                {
                    _logger.LogError("Something wrong happpened why trying to execute migrations for the RestaurantSimulationDatabase. " +
                        "Exception is {@Exception}", ex);

                    var milisecondsDelay = 5000;

                    _logger.LogInformation("Will wait {@milisecondsDelay} seconds before the migrations will try to be executed again", milisecondsDelay);

                    await Task.Delay(milisecondsDelay);
                }
            }

            _logger.LogInformation("Migration were executed sucesfully. The console application will be closed.");

            Environment.Exit(0);
        }
    }
}
