using ErrorOr;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using RestaurantSimulation.Domain.RestaurantApplicationErrors;

namespace RestaurantSimulation.Application.Authentication.Common.Services.ExtractUserClaims
{
    public class ExtractUserClaimsService : IExtractUserClaimsService
    {
        private readonly IHttpContextAccessor accessor;

        public ExtractUserClaimsService(IHttpContextAccessor accessor)
        {
            this.accessor = accessor;
        }


        private ClaimsIdentity GetClaimsIdentity()
        {
            return accessor.HttpContext?.User.Identity as ClaimsIdentity; ;
        }

        public ErrorOr<string> GetUserEmail()
        {
            Claim email = GetClaimsIdentity()?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);

            if (email is null)
                return Errors.Authentication.EmailClaimNull;

            return email.Value;
        }

        public ErrorOr<string> GetUserSub()
        {
            Claim sub = GetClaimsIdentity()?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            if (sub is null)
                return Errors.Authentication.SubClaimNull;

            return sub.Value;
        }
    }
}
