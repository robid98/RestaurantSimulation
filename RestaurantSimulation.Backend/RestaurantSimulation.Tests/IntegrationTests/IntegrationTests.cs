using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RestaurantSimulation.Infrastructure.Persistence;
using System.Dynamic;
using System.Net;
using System.Security.Claims;
using WebMotions.Fake.Authentication.JwtBearer;

namespace RestaurantSimulation.Tests.IntegrationTests
{
    public class IntegrationTests
    {
        protected readonly HttpClient TestClient;

        private readonly string _dbName = Guid.NewGuid().ToString();

        protected IntegrationTests()
        {
            var appFactory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var context = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(RestaurantSimulationContext));
                        if (context != null)
                        {
                            services.Remove(context);
                            var options = services.Where(r => (r.ServiceType == typeof(DbContextOptions))
                              || (r.ServiceType.IsGenericType && r.ServiceType.GetGenericTypeDefinition() == typeof(DbContextOptions<>))).ToArray();
                            foreach (var option in options)
                            {
                                services.Remove(option);
                            }
                        }
                        services.AddDbContext<RestaurantSimulationContext>(options =>
                        {
                            options.UseInMemoryDatabase(_dbName);
                        });

                        services.AddAuthentication(FakeJwtBearerDefaults.AuthenticationScheme).AddFakeJwtBearer();
                    });
                });

            TestClient = appFactory.CreateClient();
        }

        protected void  AuthenticateAsync()
        {
            var data = new ExpandoObject() as IDictionary<string, Object>;

            data.Add(ClaimTypes.NameIdentifier, Guid.NewGuid());
            data.Add(ClaimTypes.Email, $"test_mail{Guid.NewGuid()}@restaurant.com");

            TestClient.SetFakeBearerToken((object)data);
        }
    }
}
