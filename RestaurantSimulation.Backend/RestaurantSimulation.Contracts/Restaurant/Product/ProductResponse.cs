namespace RestaurantSimulation.Contracts.Restaurant.Product
{
    public record ProductResponse(Guid Id,
        double Price,
        string Description,
        bool isAvailable,
        Guid CategoryId);
}
