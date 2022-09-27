using ErrorOr;
using MediatR;
using RestaurantSimulation.Application.Restaurant.RestaurantMenuCategory.Common;

namespace RestaurantSimulation.Application.Restaurant.RestaurantMenuCategory.Queries.GetMenuCategories
{
    public record GetMenuCategoriesQuery() : IRequest<ErrorOr<List<MenuCategoryResult>>>;
}
