using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebMotions.Fake.Authentication.JwtBearer;
using RestaurantSimulation.Infrastructure.Persistence;

namespace RestaurantSimulation.IntegrationTests.Helpers
{
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();
        
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<RestaurantSimulationContext>));

                services.Remove(descriptor);

                var mySqlConnectionString = Configuration.GetValue<string>("ConnectionStrings:Mysql");

                services.AddDbContext<RestaurantSimulationContext>(
                    options => options.UseMySql(mySqlConnectionString,
                    ServerVersion.AutoDetect(mySqlConnectionString),
                    mySqlOptions =>
                    {
                        mySqlOptions.MigrationsAssembly(typeof(RestaurantSimulationContext).Assembly.FullName);
                    }
                ));

                services.AddAuthentication(FakeJwtBearerDefaults.AuthenticationScheme).AddFakeJwtBearer();

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;

                    var db = scopedServices.GetRequiredService<RestaurantSimulationContext>();

                    db.Database.Migrate();
                }
            });
        }
    }
}
