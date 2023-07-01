namespace RestaurantSimulation.Contracts.Restaurant.Product
{
    public record ProductResponse(Guid Id,
        double Price,
        string Name,
        string Description,
        bool isAvailable,
        Guid CategoryId);
}
