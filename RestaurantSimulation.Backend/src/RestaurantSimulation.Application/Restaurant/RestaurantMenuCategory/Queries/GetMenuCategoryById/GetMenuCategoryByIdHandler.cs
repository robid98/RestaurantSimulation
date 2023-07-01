using ErrorOr;
using MediatR;
using RestaurantSimulation.Application.Common.Interfaces.Persistence;
using RestaurantSimulation.Application.Restaurant.RestaurantMenuCategory.Common;
using RestaurantSimulation.Domain.Entities.Restaurant;
using RestaurantSimulation.Domain.RestaurantApplicationErrors;

namespace RestaurantSimulation.Application.Restaurant.RestaurantMenuCategory.Queries.GetMenuCategoryById
{
    public class GetMenuCategoryByIdHandler : IRequestHandler<GetMenuCategoryByIdQuery, ErrorOr<MenuCategoryResult>>
    {
        private readonly IMenuCategoryRepository _menuCategoryRepository;

        public GetMenuCategoryByIdHandler(IMenuCategoryRepository menuCategoryRepository)
        {
            _menuCategoryRepository = menuCategoryRepository;
        }

        public async Task<ErrorOr<MenuCategoryResult>> Handle(GetMenuCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            MenuCategory? menuCategory = await _menuCategoryRepository.GetRestaurantMenuCategoryByIdAsync(request.Id);

            if (menuCategory is null)
                return Errors.RestaurantMenuCategory.NotFound;

            return new MenuCategoryResult(menuCategory.Id, menuCategory.Name, menuCategory.Description);
        }
    }
}
