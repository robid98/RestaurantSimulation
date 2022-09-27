using ErrorOr;
using MediatR;
using RestaurantSimulation.Application.Common.Interfaces.Persistence;
using RestaurantSimulation.Domain.RestaurantApplicationErrors;

namespace RestaurantSimulation.Application.Restaurant.RestaurantMenuCategory.Commands.DeleteMenuCategory
{
    public class DeleteMenuCategoryHandler : IRequestHandler<DeleteMenuCategoryCommand, ErrorOr<Unit>>
    {
        private readonly IMenuCategoryRepository _menuCategoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteMenuCategoryHandler(
            IMenuCategoryRepository menuCategoryRepository,
            IUnitOfWork unitOfWork)
        {
            _menuCategoryRepository = menuCategoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<Unit>> Handle(DeleteMenuCategoryCommand request, CancellationToken cancellationToken)
        {
            var menuCategory = await _menuCategoryRepository.GetRestaurantMenuCategoryById(request.id);

            if (menuCategory is null)
                return Errors.RestaurantMenuCategory.NotFound;

            await _menuCategoryRepository.DeleteAsync(menuCategory);

            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
