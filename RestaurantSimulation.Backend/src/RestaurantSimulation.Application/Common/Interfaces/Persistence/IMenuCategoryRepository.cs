using RestaurantSimulation.Domain.Entities.Restaurant;

namespace RestaurantSimulation.Application.Common.Interfaces.Persistence
{
    public interface IMenuCategoryRepository
    {
        Task AddAsync(MenuCategory restaurantMenuCategory);

        Task DeleteAsync(MenuCategory request);

        Task<List<MenuCategory>> GetRestaurantMenuCategoriesAsync();

        Task<MenuCategory> GetRestaurantMenuCategoryByIdAsync(Guid id);

        Task<MenuCategory> GetRestaurantMenuCategoryByNameAsync(string name);

        Task<List<Product>> GetProductsRestaurantMenuCategoryByIdAsync(Guid id);
    }
}
