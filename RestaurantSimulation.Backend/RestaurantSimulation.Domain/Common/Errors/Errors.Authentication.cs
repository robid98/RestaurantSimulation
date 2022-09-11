using ErrorOr;

namespace RestaurantSimulation.Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class Authentication
        {
            public static Error EmailClaimNull =>
                Error.Failure(
                    code: "Auth.InvalidClaim",
                    "Email claim can not be extracted.");

            public static Error SubClaimNull =>
                Error.Failure(
                    code: "Auth.InvalidClaim",
                    "Sub claim can not be extracted.");
        }
    }
}
