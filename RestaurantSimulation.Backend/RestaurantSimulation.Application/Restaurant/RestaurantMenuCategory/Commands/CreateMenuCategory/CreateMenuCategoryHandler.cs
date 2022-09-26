using ErrorOr;
using MediatR;
using RestaurantSimulation.Application.Common.Interfaces.Persistence;
using RestaurantSimulation.Application.Restaurant.RestaurantMenuCategory.Common;
using RestaurantSimulation.Domain.Entities.Restaurant;
using RestaurantSimulation.Domain.RestaurantApplicationErrors;

namespace RestaurantSimulation.Application.Restaurant.RestaurantMenuCategory.Commands.CreateMenuCategory
{
    public class CreateMenuCategoryHandler : IRequestHandler<CreateMenuCategoryCommand, ErrorOr<MenuCategoryResult>>
    {
        private readonly IMenuCategoryRepository _menuCategoryRepository;

        public CreateMenuCategoryHandler(IMenuCategoryRepository menuCategoryRepository)
        {
            _menuCategoryRepository = menuCategoryRepository;
        }

        public async Task<ErrorOr<MenuCategoryResult>> Handle(CreateMenuCategoryCommand request, CancellationToken cancellationToken)
        {
            if (await _menuCategoryRepository.GetRestaurantCategoryByName(request.Name) is not null)
            {
                return Errors.RestaurantMenuCategory.DuplicateRestaurantMenuCategory;
            }

            var categoryId = Guid.NewGuid();

            var category = new MenuCategory(categoryId, request.Name, request.Description);

            await _menuCategoryRepository.AddAsync(category);

            return new MenuCategoryResult(
                categoryId,
                request.Name,
                request.Description);
        }
    }
}
