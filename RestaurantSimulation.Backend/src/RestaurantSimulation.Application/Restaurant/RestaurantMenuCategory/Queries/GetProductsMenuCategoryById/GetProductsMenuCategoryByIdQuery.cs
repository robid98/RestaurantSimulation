using ErrorOr;
using MediatR;
using RestaurantSimulation.Application.Restaurant.RestaurantProducts.Common;

namespace RestaurantSimulation.Application.Restaurant.RestaurantMenuCategory.Queries.GetProductsMenuCategoryById
{
    public record GetProductsMenuCategoryByIdQuery(Guid Id) : IRequest<ErrorOr<List<ProductResult>>>;
}
