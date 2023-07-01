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
        private readonly IUnitOfWork _unitOfWork;

        public UpdateMenuCategoryHandler(
            IMenuCategoryRepository menuCategoryRepository,
            IUnitOfWork unitOfWork)
        {
            _menuCategoryRepository = menuCategoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<MenuCategoryResult>> Handle(UpdateMenuCategoryCommand request, CancellationToken cancellationToken)
        {
            MenuCategory category = await _menuCategoryRepository.GetRestaurantMenuCategoryByIdAsync(request.Id);

            if (category is null)
            {
                return Errors.RestaurantMenuCategory.NotFound;
            }

            if (await _menuCategoryRepository.GetRestaurantMenuCategoryByNameAsync(request.Name) is not null)
            {
                return Errors.RestaurantMenuCategory.DuplicateRestaurantMenuCategory;
            }

            category.UpdateMenuCategoryInfo(request.Name, request.Description);

            await _unitOfWork.SaveChangesAsync();

            return new MenuCategoryResult (
                category.Id,
                category.Name,
                category.Description
            );
        }
    }
}
