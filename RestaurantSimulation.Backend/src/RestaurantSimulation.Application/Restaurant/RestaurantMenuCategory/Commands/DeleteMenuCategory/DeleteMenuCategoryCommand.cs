using ErrorOr;
using MediatR;

namespace RestaurantSimulation.Application.Restaurant.RestaurantMenuCategory.Commands.DeleteMenuCategory
{
    public record DeleteMenuCategoryCommand(
        Guid id) : IRequest<ErrorOr<Unit>>;
}
