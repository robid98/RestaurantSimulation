using RestaurantSimulation.Domain.Entities.Authentication;
using RestaurantSimulation.Domain.Entities.Restaurant;
using RestaurantSimulation.Infrastructure.Persistence;

namespace RestaurantSimulation.IntegrationTests
{
    public static class RestaurantContextSeed
    {
        public static List<Guid> usersGuid = new() { Guid.NewGuid(), Guid.NewGuid() };

        public static List<User> users = new()
        {
            new User(usersGuid[0], "auth0|user0", "user0test@gmail.com", "Gigel", "Fronea", "0773823000", "Buhusi, nr 5"),
            new User(usersGuid[1], "auth0|user1", "user1test@gmail.com", "Marinel", "Alex", "0773823001", "Buhusi, nr 3")
        };

        public static List<Guid> categoriesGuid = new() { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()};

        public static List<MenuCategory> menuCategories = new()
        {
            new MenuCategory(categoriesGuid[0], "Fruits", "Best fruits in Europa"),
            new MenuCategory(categoriesGuid[1], "Desert", "Best deserts in the World"),
            new MenuCategory(categoriesGuid[2], "Maritime", "Cele mai bune fructe de mare")
        };

        public static List<Guid> productsGuid = new() { Guid.NewGuid(), Guid.NewGuid() };

        public static List<Product> products = new()
        {
            new Product(productsGuid[0], 10.05, "Carne de vita", "Carne, cartofi", true, categoriesGuid[1]),
            new Product(productsGuid[1], 11.50, "Inghetata", "Inghetata cu de toate", true, categoriesGuid[1])
        };

        public static void SeedAsync(RestaurantSimulationContext restaurantSimulationContext)
        {
            SeedUsers(restaurantSimulationContext);
            SeedMenuCategories(restaurantSimulationContext);
            SeedProducts(restaurantSimulationContext);
        }

        private static void SeedUsers(RestaurantSimulationContext restaurantSimulationContext)
        {
            if (!restaurantSimulationContext.Users.Any())
            {
                restaurantSimulationContext.Users.AddRange(users);
                restaurantSimulationContext.SaveChanges();
            }
        }

        private static void SeedMenuCategories(RestaurantSimulationContext restaurantSimulationContext)
        {
            if (!restaurantSimulationContext.MenuCategories.Any())
            {
                restaurantSimulationContext.MenuCategories.AddRange(menuCategories);
                restaurantSimulationContext.SaveChanges();
            }
        }

        private static void SeedProducts(RestaurantSimulationContext restaurantSimulationContext)
        {
            if (!restaurantSimulationContext.Products.Any())
            {
                restaurantSimulationContext.Products.AddRange(products);
                restaurantSimulationContext.SaveChanges();
            }
        }
    }
}
