using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.OpenApi.Models;
using RestaurantSimulation.Api.Common.Errors;

namespace RestaurantSimulation.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddSingleton<ProblemDetailsFactory, RestaurantSimulationProblemDetailsFactory>();
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(configuration);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            return services;
        }


        public static IServiceCollection AddSwaggerGen(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "API Documentation",
                    Version = "v1.0",
                    Description = "RestaurantSimulation.Api"
                });
                options.ResolveConflictingActions(x => x.First());
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    BearerFormat = "JWT",
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            TokenUrl = new Uri($"https://{configuration["Auth0:Domain"]}/oauth/token"),
                            AuthorizationUrl = new Uri($"https://{configuration["Auth0:Domain"]}/authorize?audience={configuration["Auth0:Audience"]}"),
                            Scopes = new Dictionary<string, string>
                            {
                                { "openid", "OpenId" },

                            }
                        }
                    }
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                      {
                          new OpenApiSecurityScheme
                          {
                              Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
                          },
                          new[] { "openid" }
                      }
                });
            });

            return services;
        }

    }
}
