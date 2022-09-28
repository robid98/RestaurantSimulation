using RestaurantSimulation.Application.Common.Interfaces.Persistence;
using RestaurantSimulation.Domain.Entities.Restaurant;
using Microsoft.EntityFrameworkCore;

namespace RestaurantSimulation.Infrastructure.Persistence.Restaurant
{
    public class MenuCategoryRepository : IMenuCategoryRepository
    {
        private readonly RestaurantSimulationContext _restaurantSimulationContext;

        public MenuCategoryRepository(RestaurantSimulationContext restaurantSimulationContext)
        {
            _restaurantSimulationContext = restaurantSimulationContext;
        }


        public async Task AddAsync(MenuCategory category)
        {
            await _restaurantSimulationContext.MenuCategories.AddAsync(category);
        }

        public async Task DeleteAsync(MenuCategory request)
        {
            await Task.CompletedTask;

            _restaurantSimulationContext.MenuCategories.Remove(request);
        }

        public async Task<List<MenuCategory>> GetRestaurantMenuCategoriesAsync()
        {
            List<MenuCategory> menuCategories = await _restaurantSimulationContext.MenuCategories.ToListAsync();

            return menuCategories;
        }

        public async Task<MenuCategory?> GetRestaurantMenuCategoryByIdAsync(Guid id)
        {
            MenuCategory? category = await _restaurantSimulationContext.MenuCategories.FirstOrDefaultAsync(category => category.Id == id);

            return category;
        }

        public async Task<List<Product>?> GetProductsRestaurantMenuCategoryByIdAsync(Guid id)
        {
            MenuCategory? category = await _restaurantSimulationContext.MenuCategories
                .Include(g => g.Products).FirstOrDefaultAsync(category => category.Id == id);

            return category?.Products.ToList();
        }

        public async Task<MenuCategory?> GetRestaurantMenuCategoryByNameAsync(string name)
        {
            MenuCategory? category = await _restaurantSimulationContext.MenuCategories.FirstOrDefaultAsync(category => category.Name == name);

            return category;
        }
    }
}
