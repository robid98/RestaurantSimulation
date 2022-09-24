using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using RestaurantSimulation.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using RestaurantSimulation.Application.Common.Interfaces.Persistence;
using RestaurantSimulation.Infrastructure.Persistence.Authentication;
using RestaurantSimulation.Domain.Common.Policies.Authorization;
using RestaurantSimulation.Domain.Common.Claims;
using RestaurantSimulation.Domain.Common.Roles;
using RestaurantSimulation.Infrastructure.Persistence.Restaurant;

namespace RestaurantSimulation.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddAuthenticationServices(configuration)
                .AddAuthorizationServices(configuration)
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

        public static IServiceCollection AddAuthorizationServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(AuthorizationPolicies.AdminRolePolicy,
                    (policy) => {
                        policy.RequireClaim(RestaurantSimulationClaims.RestaurantSimulationRoles, RestaurantSimulationRoles.AdminRole);
                    });

                options.AddPolicy(AuthorizationPolicies.ClientRolePolicy,
                    (policy) => {
                        policy.RequireClaim(RestaurantSimulationClaims.RestaurantSimulationRoles, RestaurantSimulationRoles.ClientRole);
                    });

                options.AddPolicy(AuthorizationPolicies.ClientOrAdminRolePolicy,
                    (policy) => {
                        policy.RequireClaim(RestaurantSimulationClaims.RestaurantSimulationRoles, RestaurantSimulationRoles.ClientRole, RestaurantSimulationRoles.AdminRole);
                    });

            });

            return services;
        }

        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddDbContext<RestaurantSimulationContext>(options =>
                options.UseSqlServer(configuration["SqlServer:ConnectionString"], 
                optionsAction => optionsAction.MigrationsAssembly("RestaurantSimulation.Infrastructure")));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IMenuCategoryRepository, MenuCategoryRepository>();

            return services;
        }
    }
}
