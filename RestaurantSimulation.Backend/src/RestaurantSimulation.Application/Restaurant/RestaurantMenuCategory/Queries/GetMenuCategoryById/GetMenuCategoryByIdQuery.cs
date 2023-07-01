using ErrorOr;
using MediatR;
using RestaurantSimulation.Application.Restaurant.RestaurantMenuCategory.Common;

namespace RestaurantSimulation.Application.Restaurant.RestaurantMenuCategory.Queries.GetMenuCategoryById
{
    public record GetMenuCategoryByIdQuery(Guid Id) : IRequest<ErrorOr<MenuCategoryResult>>;
}
