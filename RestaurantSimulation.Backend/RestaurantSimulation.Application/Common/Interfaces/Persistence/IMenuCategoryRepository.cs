using RestaurantSimulation.Domain.Entities.Restaurant;

namespace RestaurantSimulation.Application.Common.Interfaces.Persistence
{
    public interface IMenuCategoryRepository
    {
        Task AddAsync(MenuCategory restaurantMenuCategory);

        Task DeleteAsync(MenuCategory request);

        Task<List<MenuCategory>> GetRestaurantMenuCategories();

        Task<MenuCategory?> GetRestaurantMenuCategoryById(Guid id);

        Task<MenuCategory?> GetRestaurantMenuCategoryByName(string name);

        Task<List<Product>?> GetProductsRestaurantMenuCategoryById(Guid id);
    }
}
