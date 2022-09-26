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
        private readonly IUnitOfWork _unitOfWork;

        public CreateMenuCategoryHandler(
            IMenuCategoryRepository menuCategoryRepository,
            IUnitOfWork unitOfWork)
        {
            _menuCategoryRepository = menuCategoryRepository;
            _unitOfWork = unitOfWork;
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

            await _unitOfWork.SaveChangesAsync();

            return new MenuCategoryResult(
                categoryId,
                request.Name,
                request.Description);
        }
    }
}
