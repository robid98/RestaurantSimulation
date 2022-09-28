using ErrorOr;
using MediatR;
using RestaurantSimulation.Application.Common.Interfaces.Persistence;
using RestaurantSimulation.Application.Restaurant.RestaurantProducts.Common;
using RestaurantSimulation.Domain.Entities.Restaurant;
using RestaurantSimulation.Domain.RestaurantApplicationErrors;

namespace RestaurantSimulation.Application.Restaurant.RestaurantMenuCategory.Queries.GetProductsMenuCategoryById
{
    public class GetProductsMenuCategoryByIdHandler : IRequestHandler<GetProductsMenuCategoryByIdQuery, ErrorOr<List<ProductResult>>>
    {
        private readonly IMenuCategoryRepository _menuCategoryRepository;

        public GetProductsMenuCategoryByIdHandler(IMenuCategoryRepository menuCategoryRepository)
        {
            _menuCategoryRepository = menuCategoryRepository;
        }

        public async Task<ErrorOr<List<ProductResult>>> Handle(GetProductsMenuCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            List<Product>? products = await _menuCategoryRepository.GetProductsRestaurantMenuCategoryByIdAsync(request.Id);

            if (products is null)
                return Errors.RestaurantMenuCategory.NotFound;

            return products.Select(product => new ProductResult(
                product.Id, 
                product.Price, 
                product.Description, 
                product.IsAvailable, 
                product.CategoryId)).ToList();
        }
    }
}
