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

        public string Name { get; private set; }

        public string Description { get; private set; }

        public virtual ICollection<Product> Products { get; private set; }

        public void UpdateMenuCategoryInfo(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
