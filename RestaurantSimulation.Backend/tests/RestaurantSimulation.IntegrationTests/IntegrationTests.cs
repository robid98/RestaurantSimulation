using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RestaurantSimulation.Application.Common.Interfaces.Persistence;
using RestaurantSimulation.Domain.Common.Claims;
using RestaurantSimulation.Infrastructure.Persistence;
using System.Dynamic;
using System.Net;
using System.Security.Claims;
using WebMotions.Fake.Authentication.JwtBearer;

namespace RestaurantSimulation.IntegrationTests
{
    public class IntegrationTestsSetup : IDisposable
    {
        public readonly HttpClient TestClient;

        private RestaurantSimulationContext _context { get; set; }

        private SqliteConnection Connection;

        public string userSub = Guid.NewGuid().ToString();

        public IntegrationTestsSetup()
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

                        var unitOfWork = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(IUnitOfWork));
                        if (unitOfWork != null)
                        {
                            services.Remove(unitOfWork);    
                        }


                        // Microsoft.EntityFrameworkCore.Sqlite needs to be installed.
                        Connection = new SqliteConnection("DataSource=:memory:");
                        Connection.Open();

                        services.AddDbContext<RestaurantSimulationContext>(options => options.UseSqlite(Connection));

                        services.AddScoped<IUnitOfWork, UnitOfWork>();

                        services.AddAuthentication(FakeJwtBearerDefaults.AuthenticationScheme).AddFakeJwtBearer();

                        var serviceProvider = services.BuildServiceProvider();
                        _context = serviceProvider.GetService<RestaurantSimulationContext>();
                        _context?.Database.EnsureCreated();

                        RestaurantContextSeed.SeedAsync(_context!);
                    });
                });

            TestClient = appFactory.CreateClient();
        }

        public void Dispose()
        {
            _context?.Database.EnsureDeleted();
        }

        public void  AuthenticateAsync(string role, string email, string userSub)
        {
            var data = new ExpandoObject() as IDictionary<string, Object>;

            data.Add(ClaimTypes.NameIdentifier, userSub);
            data.Add(ClaimTypes.Email, email);
            data.Add(RestaurantSimulationClaims.RestaurantSimulationRoles, role);

            TestClient.SetFakeBearerToken((object)data);
        }
    }
}
