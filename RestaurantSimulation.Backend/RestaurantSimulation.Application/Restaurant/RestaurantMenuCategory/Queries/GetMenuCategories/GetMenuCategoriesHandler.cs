using ErrorOr;
using MediatR;
using RestaurantSimulation.Application.Common.Interfaces.Persistence;
using RestaurantSimulation.Application.Restaurant.RestaurantMenuCategory.Common;
using RestaurantSimulation.Domain.Entities.Restaurant;

namespace RestaurantSimulation.Application.Restaurant.RestaurantMenuCategory.Queries.GetMenuCategories
{
    public class GetMenuCategoriesHandler : IRequestHandler<GetMenuCategoriesQuery, ErrorOr<List<MenuCategoryResult>>>
    {
        private readonly IMenuCategoryRepository _menuCategoryRepository;

        public GetMenuCategoriesHandler(IMenuCategoryRepository menuCategoryRepository)
        {
            _menuCategoryRepository = menuCategoryRepository;
        }

        public async Task<ErrorOr<List<MenuCategoryResult>>> Handle(GetMenuCategoriesQuery request, CancellationToken cancellationToken)
        {
            List<MenuCategory> menuCategories = await _menuCategoryRepository.GetRestaurantMenuCategoriesAsync();

            return menuCategories.Select(menuCategory => new MenuCategoryResult(menuCategory.Id,
                menuCategory.Name,
                menuCategory.Description)).ToList();
        }
    }
}
