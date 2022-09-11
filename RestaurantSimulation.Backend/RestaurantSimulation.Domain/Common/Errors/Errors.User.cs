using ErrorOr;

namespace RestaurantSimulation.Domain.Common.Errors
{ 
    public static partial class Errors
    {
        public static class User
        {
            public static Error DuplicateEmail => 
                Error.Conflict(
                    code: "User.DuplicateEmail", 
                    "Email is already in use");

        }
    }
}
