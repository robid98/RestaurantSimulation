using Microsoft.EntityFrameworkCore;
using RestaurantSimulation.Domain.Entities.Authentication;
using RestaurantSimulation.Domain.Entities.Restaurant;
using RestaurantSimulation.Infrastructure.Persistence.RelationshipsEntityFramework;
using RestaurantSimulation.Infrastructure.Persistence.Seeding;

namespace RestaurantSimulation.Infrastructure.Persistence
{
    public class RestaurantSimulationContext : DbContext
    {
        public RestaurantSimulationContext(DbContextOptions<RestaurantSimulationContext> options) : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ProductMenuCategoryRelationship();
        }

        public static void Seed(RestaurantSimulationContext restaurantSimulationContext)
        {
            MenuCategoriesSeed.Seed(restaurantSimulationContext);
            restaurantSimulationContext.SaveChanges();
        }

        public DbSet<User> Users { get; set; } = default!;
        public DbSet<Product> Products { get; set; } = default!;
        public DbSet<MenuCategory> MenuCategories { get; set; } = default!;
    }
}
