using Microsoft.EntityFrameworkCore;
using RestaurantSimulation.Domain.Entities.Restaurant;

namespace RestaurantSimulation.Infrastructure.Persistence.RelationshipsEntityFramework
{
    public static class OneToMany
    {
        public static void ProductMenuCategoryRelationship(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MenuCategory>()
                .HasMany<Product>(g => g.Products)
                .WithOne(s => s.Category)
                .HasForeignKey(s => s.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
