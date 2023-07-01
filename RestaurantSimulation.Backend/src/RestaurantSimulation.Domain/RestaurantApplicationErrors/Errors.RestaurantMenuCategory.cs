using ErrorOr;

namespace RestaurantSimulation.Domain.RestaurantApplicationErrors
{
    public static partial class Errors
    {
        public static class RestaurantMenuCategory
        {
            public static Error NotFound =>
                Error.NotFound(
                    code: "RestaurantMenuCategory.NotFound",
                    "Restaurant Menu Category was not found in the Restaurant Simulation.");

            public static Error DuplicateRestaurantMenuCategory =>
                Error.Conflict(
                    code: "RestaurantMenuCategory.DuplicateName",
                    "This Restaurant Menu Category already exists.");
        }
    }
}
