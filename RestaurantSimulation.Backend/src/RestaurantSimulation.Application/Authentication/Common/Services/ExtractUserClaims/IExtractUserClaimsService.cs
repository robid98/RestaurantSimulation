using ErrorOr;

namespace RestaurantSimulation.Application.Authentication.Common.Services.ExtractUserClaims
{
    public interface IExtractUserClaimsService
    {
        public ErrorOr<string> GetUserEmail();

        public ErrorOr<string> GetUserSub();
    }
}
