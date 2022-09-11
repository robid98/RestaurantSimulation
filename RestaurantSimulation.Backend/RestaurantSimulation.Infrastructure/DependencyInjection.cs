using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using RestaurantSimulation.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using RestaurantSimulation.Application.Common.Interfaces.Persistence;
using RestaurantSimulation.Infrastructure.Persistence.Authentication;

namespace RestaurantSimulation.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddAuthenticationServices(configuration)
                .AddPersistenceServices(configuration);

            return services;
        }

        public static IServiceCollection AddAuthenticationServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            string domain = $"https://{configuration["Auth0:Domain"]}/";
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = domain;
                    options.Audience = configuration["Auth0:Audience"];
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = ClaimTypes.NameIdentifier
                    };
                });

            return services;
        }

        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddDbContext<RestaurantSimulationContext>(options =>
                options.UseSqlServer(configuration["SqlServer:ConnectionString"], 
                optionsAction => optionsAction.MigrationsAssembly("RestaurantSimulation.Infrastructure")));

            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
