using RestaurantSimulation.Domain.Primitives;

namespace RestaurantSimulation.Domain.Entities.Restaurant
{
    public class MenuCategory : Entity
    {
        public MenuCategory(Guid id, string name, string description) : base(id)
        {
            Name = name;
            Description = description;
        }

        public string Name { get; private set; } = default!;

        public string Description { get; private set; } = default!;

        public ICollection<Product> Products { get; private set; } = default!;

        public static List<MenuCategory> RestaurantCategories = new()
        {
            new MenuCategory(Guid.NewGuid(), "Breakfast", "Here you will have all the dishes you can eat in the morning."),
            new MenuCategory(Guid.NewGuid(), "Salads", "Here you will find among the best salads."),
            new MenuCategory(Guid.NewGuid(), "Seafood", "Quality seafood and fish dishes."),
            new MenuCategory(Guid.NewGuid(), "Desserts", "Life is short, eat the dessert."),
            new MenuCategory(Guid.NewGuid(), "Drinks", "Hot and Cold drinks for everyone."),
            new MenuCategory(Guid.NewGuid(), "Pizzas", "Here you will find all kinds of pizza."),
            new MenuCategory(Guid.NewGuid(), "Grill ", "Here you will find dishes cooked on the grill."),
        };

        public void UpdateMenuCategoryInfo(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
