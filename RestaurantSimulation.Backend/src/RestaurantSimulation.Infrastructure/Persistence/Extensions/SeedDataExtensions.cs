using Microsoft.EntityFrameworkCore;
using RestaurantSimulation.Domain.Entities.Restaurant;

namespace RestaurantSimulation.Infrastructure.Persistence.Extensions
{
    public static class SeedDataExtensions
    {
        public static void AddSeedData(this ModelBuilder modelBuilder)
        {
            modelBuilder.SeedMenuCategory();
        }

        private static void SeedMenuCategory(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MenuCategory>().HasData(
                new MenuCategory(Guid.Parse("694d6ed1-4ef5-4539-926d-c459c2ba1b39"), "Breakfast", "Here you will have all the dishes you can eat in the morning."),
                new MenuCategory(Guid.Parse("d63d9f83-eb6a-4aa0-b7e5-64370978c8c1"), "Salads", "Here you will find among the best salads."),
                new MenuCategory(Guid.Parse("7dd4b57d-3601-43d6-bf7a-86710f613d45"), "Seafood", "Quality seafood and fish dishes."),
                new MenuCategory(Guid.Parse("503595d7-3306-4f09-b1ab-73beb764cf93"), "Desserts", "Life is short, eat the dessert."),
                new MenuCategory(Guid.Parse("ee920cbc-bcef-4ac5-9630-3c87321e5b76"), "Drinks", "Hot and Cold drinks for everyone."),
                new MenuCategory(Guid.Parse("f63587f5-0da9-4517-80e5-2e3035f0ba19"), "Pizzas", "Here you will find all kinds of pizza."),
                new MenuCategory(Guid.Parse("02c63d08-fc05-493e-add6-00e56eb74686"), "Grill ", "Here you will find dishes cooked on the grill.")
            );
        }
    }
}
