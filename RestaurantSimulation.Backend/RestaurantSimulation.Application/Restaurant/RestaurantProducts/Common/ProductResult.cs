namespace RestaurantSimulation.Application.Restaurant.RestaurantProducts.Common
{
    public record ProductResult(Guid Id,
        double Price,
        string Description,
        bool isAvailable,
        Guid CategoryId);
}
