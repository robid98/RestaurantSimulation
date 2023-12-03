using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using ErrorOr;
using RestaurantSimulation.Domain.RestaurantApplicationErrors;
using Microsoft.Extensions.Logging;
using RestaurantSimulation.Domain.Common.Claims;

namespace RestaurantSimulation.Application.Authentication.Common.Services.ExtractUserClaims
{
    public class ExtractUserClaimsService : IExtractUserClaimsService
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly ILogger<ExtractUserClaimsService> _logger;

        public ExtractUserClaimsService(
            IHttpContextAccessor accessor,
            ILogger<ExtractUserClaimsService> logger)
        {
            _accessor = accessor;
            _logger = logger;
        }


        private ClaimsIdentity GetClaimsIdentity()
        {
            return _accessor.HttpContext?.User.Identity as ClaimsIdentity; ;
        }

        public ErrorOr<string> GetUserEmail()
        {
            Claim email = GetClaimsIdentity()?.Claims.FirstOrDefault(x => x.Type == RestaurantSimulationClaims.Email);

            if (email is null)
            {
                _logger.LogError("Email can not be extracted from the current User. Error returned {@RestaurantSimulationDomainError}", Errors.Authentication.EmailClaimNull);

                return Errors.Authentication.EmailClaimNull;
            }

            return email.Value;
        }

        public ErrorOr<string> GetUserSub()
        {
            Claim sub = GetClaimsIdentity()?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            if (sub is null)
            {
                _logger.LogError("Sub can not be extracted from the current User. Error returned {@RestaurantSimulationDomainError}", Errors.Authentication.SubClaimNull);

                return Errors.Authentication.SubClaimNull;
            }

            return sub.Value;
        }
    }
}
