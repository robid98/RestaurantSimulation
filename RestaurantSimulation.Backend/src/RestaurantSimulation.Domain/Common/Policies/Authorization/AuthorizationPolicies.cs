namespace RestaurantSimulation.Domain.Common.Policies.Authorization
{
    public class AuthorizationPolicies
    {
        public const string AdminRolePolicy = "AdminRolePolicy";

        public const string ClientRolePolicy = "ClientRolePolicy";

        public const string ClientOrAdminRolePolicy = "ClientOrAdminRolePolicy";
    }
}
