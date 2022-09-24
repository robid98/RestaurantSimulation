using ErrorOr;
using MediatR;
using RestaurantSimulation.Application.Common.Interfaces.Persistence;
using RestaurantSimulation.Application.Restaurant.RestaurantMenuCategory.Common;
using RestaurantSimulation.Domain.Entities.Restaurant;

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
            var menuCategory = await _menuCategoryRepository.UpdateAsync(new MenuCategory { 
                Id = request.Id,
                Name = request.Name,
                Description = request.Description,
            });

            if (menuCategory.IsError)
                return menuCategory.FirstError;

            return new MenuCategoryResult (
                menuCategory.Value.Id,
                menuCategory.Value.Name,
                menuCategory.Value.Description
            );
        }
    }
}
