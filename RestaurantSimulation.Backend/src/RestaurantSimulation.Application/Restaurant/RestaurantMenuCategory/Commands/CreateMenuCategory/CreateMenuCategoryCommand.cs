using ErrorOr;
using MediatR;
using RestaurantSimulation.Application.Restaurant.RestaurantMenuCategory.Common;

namespace RestaurantSimulation.Application.Restaurant.RestaurantMenuCategory.Commands.CreateMenuCategory
{
    public record CreateMenuCategoryCommand(
        string Name,
        string Description) : IRequest<ErrorOr<MenuCategoryResult>>;
}
