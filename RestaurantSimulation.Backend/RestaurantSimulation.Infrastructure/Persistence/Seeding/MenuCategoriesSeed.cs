using RestaurantSimulation.Domain.Entities.Restaurant;

namespace RestaurantSimulation.Infrastructure.Persistence.Seeding
{
    public static class MenuCategoriesSeed
    {
        public static void Seed(RestaurantSimulationContext restaurantSimulationContext)
        {
            if (!restaurantSimulationContext.MenuCategories.Any())
            {
                restaurantSimulationContext.MenuCategories.AddRange(MenuCategory.RestaurantCategories);
            }
        }  
    }
}
