namespace RestaurantSimulation.Domain.Entities.Restaurant
{
    public class MenuCategory
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = default!;

        public string Description { get; set; } = default!;

        public virtual ICollection<Product> Products { get; set; } = default!;
    }
}
