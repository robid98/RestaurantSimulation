using Microsoft.EntityFrameworkCore;
using RestaurantSimulation.Domain.Entities.Authentication;
using RestaurantSimulation.Domain.Entities.Restaurant;
using RestaurantSimulation.Infrastructure.Persistence.Extensions;

namespace RestaurantSimulation.Infrastructure.Persistence
{
    public class RestaurantSimulationContext : DbContext
    {
        public RestaurantSimulationContext(DbContextOptions<RestaurantSimulationContext> options) : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(RestaurantSimulationContext).Assembly);

            modelBuilder.AddSeedData();
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<MenuCategory> MenuCategories { get; set; }
    }
}
