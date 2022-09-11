using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RestaurantSimulation.Application.Authentication.Common.Services.ExtractUserClaims;

namespace RestaurantSimulation.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(typeof(DependencyInjection).Assembly);
            services.AddSingleton<IExtractUserClaimsService, ExtractUserClaimsService>();

            return services;
        }
    }
}
