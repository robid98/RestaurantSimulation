using ErrorOr;
using MediatR;
using RestaurantSimulation.Application.Common.Interfaces.Persistence;
using RestaurantSimulation.Application.Restaurant.RestaurantMenuCategory.Common;
using RestaurantSimulation.Domain.Common.Errors;

namespace RestaurantSimulation.Application.Restaurant.RestaurantMenuCategory.Commands.CreateMenuCategory
{
    public class CreateMenuCategoryHandler : IRequestHandler<CreateMenuCategoryCommand, ErrorOr<MenuCategoryResult>>
    {
        private readonly IMenuCategoryRepository _restaurantMenuCategoryRepository;

        public CreateMenuCategoryHandler(IMenuCategoryRepository restaurantMenuCategoryRepository)
        {
            _restaurantMenuCategoryRepository = restaurantMenuCategoryRepository;
        }

        public async Task<ErrorOr<MenuCategoryResult>> Handle(CreateMenuCategoryCommand request, CancellationToken cancellationToken)
        {
            if (await _restaurantMenuCategoryRepository.GetRestaurantCategoryByName(request.Name) is not null)
            {
                return Errors.RestaurantMenuCategory.DuplicateRestaurantMenuCategory;
            }

            var categoryId = Guid.NewGuid();

            var category = new Domain.Entities.Restaurant.MenuCategory
            {
                Id = categoryId,
                Name = request.Name,
                Description = request.Description
            };

            await _restaurantMenuCategoryRepository.AddAsync(category);

            return new MenuCategoryResult(
                categoryId,
                request.Name,
                request.Description);
        }
    }
}
