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

        public void UpdateMenuCategoryInfo(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
