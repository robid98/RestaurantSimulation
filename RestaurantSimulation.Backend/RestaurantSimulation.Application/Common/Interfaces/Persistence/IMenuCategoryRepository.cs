using ErrorOr;
using RestaurantSimulation.Domain.Entities.Restaurant;

namespace RestaurantSimulation.Application.Common.Interfaces.Persistence
{
    public interface IMenuCategoryRepository
    {
        Task AddAsync(MenuCategory restaurantMenuCategory);

        Task UpdateAsync();

        Task DeleteAsync(MenuCategory request);

        Task<List<MenuCategory>> GetRestaurantMenuCategories();

        Task<MenuCategory?> GetRestaurantMenuCategory(Guid id);

        Task<MenuCategory?> GetRestaurantCategoryByName(string name);

        Task<List<Product>> GetProductsRestaurantMenuCategory(Guid id);
    }
}
