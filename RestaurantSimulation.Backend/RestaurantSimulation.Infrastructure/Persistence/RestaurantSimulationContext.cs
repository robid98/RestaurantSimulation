using Microsoft.EntityFrameworkCore;
using RestaurantSimulation.Domain.Entities.Authentication;

namespace RestaurantSimulation.Infrastructure.Persistence
{
    public class RestaurantSimulationContext : DbContext
    {
        public RestaurantSimulationContext(DbContextOptions<RestaurantSimulationContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; } = default!;
    }
}
