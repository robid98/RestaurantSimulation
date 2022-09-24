namespace RestaurantSimulation.Domain.Entities.Restaurant
{
    public class Product
    {
        public Guid Id { get; set; }

        public double Price { get; set; }

        public string Description { get; set; } = default!;

        public bool IsAvailable { get; set; }

        public Guid CategoryId { get; set; }

        public MenuCategory Category { get; set; } = default!;

    }
}
