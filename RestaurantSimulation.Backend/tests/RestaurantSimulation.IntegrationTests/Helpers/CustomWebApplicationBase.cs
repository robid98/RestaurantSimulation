using Microsoft.Extensions.Configuration;
using System.Dynamic;
using System.Net;
using System.Security.Claims;
using AutoFixture;
using MySqlConnector;
using Respawn;
using Respawn.Graph;
using RestaurantSimulation.Domain.Common.Claims;

namespace RestaurantSimulation.IntegrationTests.Helpers
{
    public abstract class CustomWebApplicationBase : IClassFixture<CustomWebApplicationFactory<Program>>, IDisposable
    {
        private readonly CustomWebApplicationFactory<Program> _factory;

        protected readonly HttpClient _httpClient;
        protected readonly IFixture _fixture;
        protected readonly string _userSub = Guid.NewGuid().ToString();
        protected readonly Respawner _respawner;

        public MySqlConnection _connection;

        protected CustomWebApplicationBase(CustomWebApplicationFactory<Program> factory)
        {
            _fixture = new Fixture();
            _factory = factory;
            _httpClient = _factory.CreateClient();
            _connection = new MySqlConnection(CustomWebApplicationFactory<Program>.Configuration.GetValue<string>("ConnectionStrings:Mysql"));
            _connection.Open();
            _respawner = Respawner.CreateAsync(
                _connection,
                new RespawnerOptions
                {
                    TablesToIgnore = new Table[]
                    {
                        "__EFMigrationsHistory"
                    },
                    DbAdapter = DbAdapter.MySql
                }).GetAwaiter().GetResult();
        }

        public void Dispose()
        {
            _connection.Close();
            _connection.Dispose();
            _connection = null;
        }

        public void AuthenticateAsync(string role, string email, string userSub)
        {
            var data = new ExpandoObject() as IDictionary<string, Object>;

            data.Add(ClaimTypes.NameIdentifier, userSub);
            data.Add(ClaimTypes.Email, email);
            data.Add(RestaurantSimulationClaims.RestaurantSimulationRoles, role);

            _httpClient.SetFakeBearerToken((object)data);
        }
    }
}
