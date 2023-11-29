using ErrorOr;

namespace RestaurantSimulation.Application.Authentication.Common.Services.ExtractUserClaims
{
    public interface IExtractUserClaimsService
    {
        /// <summary>
        /// Extracting user email
        /// </summary>
        /// <returns>The extracted email. Should be available for any User, otherwise an error will be returned.</returns>
        public ErrorOr<string> GetUserEmail();

        /// <summary>
        /// Extracting user sub. Unique for every registered account. Is comming from the external identity provider Auth0.
        /// </summary>
        /// <returns>The extracted sub. Should be available for any User, otherwise an error will be returned.</returns>
        public ErrorOr<string> GetUserSub();
    }
}
