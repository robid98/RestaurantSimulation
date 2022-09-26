using RestaurantSimulation.Application.Common.Interfaces.Persistence;
using RestaurantSimulation.Domain.Entities.Restaurant;
using RestaurantSimulation.Domain.RestaurantApplicationErrors;
using ErrorOr;
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

            await _restaurantSimulationContext.SaveChangesAsync();
        }

        public async Task UpdateAsync()
        {
            await _restaurantSimulationContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(MenuCategory request)
        {
            _restaurantSimulationContext.MenuCategories.Remove(request);

            await _restaurantSimulationContext.SaveChangesAsync();
        }

        public async Task<List<MenuCategory>> GetRestaurantMenuCategories()
        {
            List<MenuCategory> menuCategories = await _restaurantSimulationContext.MenuCategories.ToListAsync();

            return menuCategories;
        }

        public async Task<MenuCategory?> GetRestaurantMenuCategory(Guid id)
        {
            MenuCategory? category = await _restaurantSimulationContext.MenuCategories.FirstOrDefaultAsync(category => category.Id == id);

            return category;
        }

        public async Task<List<Product>> GetProductsRestaurantMenuCategory(Guid id)
        {
            MenuCategory? category = await _restaurantSimulationContext.MenuCategories.FirstOrDefaultAsync(category => category.Id == id);

            if (category is not null)
                return category.Products.ToList();

            return null!;
        }

        public async Task<MenuCategory?> GetRestaurantCategoryByName(string name)
        {
            MenuCategory? category = await _restaurantSimulationContext.MenuCategories.FirstOrDefaultAsync(category => category.Name == name);

            return category;
        }
    }
}
