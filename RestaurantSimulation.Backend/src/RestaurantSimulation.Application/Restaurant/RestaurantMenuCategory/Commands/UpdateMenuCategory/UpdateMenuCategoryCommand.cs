using ErrorOr;
using MediatR;
using RestaurantSimulation.Application.Restaurant.RestaurantMenuCategory.Common;

namespace RestaurantSimulation.Application.Restaurant.RestaurantMenuCategory.Commands.UpdateMenuCategory
{
    public record UpdateMenuCategoryCommand(
        Guid Id,
        string Name,
        string Description
    ) : IRequest<ErrorOr<MenuCategoryResult>>;
}
