using RestaurantSimulation.Domain.Primitives;

namespace RestaurantSimulation.Domain.Entities.Restaurant
{
    public class Product : Entity
    {
        public Product(Guid id, double price, string name, string description, bool isAvailable, Guid categoryId): base(id)
        {
            Price = price;
            Name = name;
            Description = description;
            IsAvailable = isAvailable;
            CategoryId = categoryId;
        }

        public double Price { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public bool IsAvailable { get; private set; }

        public virtual Guid CategoryId { get; private set; }

        public virtual MenuCategory Category { get; private set; }

    }
}
