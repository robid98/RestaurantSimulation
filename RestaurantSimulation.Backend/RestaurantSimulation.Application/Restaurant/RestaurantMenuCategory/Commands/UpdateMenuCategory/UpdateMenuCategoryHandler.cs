using ErrorOr;
using MediatR;
using RestaurantSimulation.Application.Common.Interfaces.Persistence;
using RestaurantSimulation.Application.Restaurant.RestaurantMenuCategory.Common;
using RestaurantSimulation.Domain.Entities.Restaurant;
using RestaurantSimulation.Domain.RestaurantApplicationErrors;

namespace RestaurantSimulation.Application.Restaurant.RestaurantMenuCategory.Commands.UpdateMenuCategory
{
    public class UpdateMenuCategoryHandler : IRequestHandler<UpdateMenuCategoryCommand, ErrorOr<MenuCategoryResult>>
    {
        private readonly IMenuCategoryRepository _menuCategoryRepository;

        public UpdateMenuCategoryHandler(IMenuCategoryRepository menuCategoryRepository)
        {
            _menuCategoryRepository = menuCategoryRepository;
        }

        public async Task<ErrorOr<MenuCategoryResult>> Handle(UpdateMenuCategoryCommand request, CancellationToken cancellationToken)
        {
            MenuCategory? category = await _menuCategoryRepository.GetRestaurantMenuCategory(request.Id);

            if (category is null)
            {
                return Errors.RestaurantMenuCategory.NotFound;
            }

            if (await _menuCategoryRepository.GetRestaurantCategoryByName(request.Name) is not null)
            {
                return Errors.RestaurantMenuCategory.DuplicateRestaurantMenuCategory;
            }

            MenuCategory.UpdateMenuCategoryInfo(category, request.Name, request.Description);

            await _menuCategoryRepository.UpdateAsync();

            return new MenuCategoryResult (
                category.Id,
                category.Name,
                category.Description
            );
        }
    }
}
