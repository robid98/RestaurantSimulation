using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantSimulation.Application.Restaurant.RestaurantMenuCategory.Commands.CreateMenuCategory;
using RestaurantSimulation.Application.Restaurant.RestaurantMenuCategory.Commands.DeleteMenuCategory;
using RestaurantSimulation.Application.Restaurant.RestaurantMenuCategory.Commands.UpdateMenuCategory;
using RestaurantSimulation.Application.Restaurant.RestaurantMenuCategory.Common;
using RestaurantSimulation.Application.Restaurant.RestaurantMenuCategory.Queries.GetMenuCategories;
using RestaurantSimulation.Application.Restaurant.RestaurantMenuCategory.Queries.GetMenuCategoryById;
using RestaurantSimulation.Application.Restaurant.RestaurantMenuCategory.Queries.GetProductsMenuCategoryById;
using RestaurantSimulation.Application.Restaurant.RestaurantProducts.Common;
using RestaurantSimulation.Contracts.Restaurant.MenuCategory;
using RestaurantSimulation.Contracts.Restaurant.Product;
using RestaurantSimulation.Domain.Common.Policies.Authorization;

namespace RestaurantSimulation.Api.Controllers
{
    [Route("api")]
    public class MenuCategoryController : ApiController
    {
        private readonly ISender _sender;

        public MenuCategoryController(
            ISender mediator)
        {
            _sender = mediator;
        }

        /// <summary>
        /// Create a new Restaurant Menu Category (admin role)
        /// </summary>
        [Authorize(Policy = AuthorizationPolicies.AdminRolePolicy)]
        [HttpPost("menucategory")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MenuCategoryResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateMenuCategory(MenuCategoryRequest request)
        {
            ErrorOr<MenuCategoryResult> createMenuCategoryCommand = await _sender.Send(
                new CreateMenuCategoryCommand(
                    request.Name,
                    request.Description)
                );

            return createMenuCategoryCommand.Match(
                createCategoryResult => Ok(GetRestaurantMenuCategoryResponse(createCategoryResult)),
                errors => Problem(errors));
        }

        /// <summary>
        /// Update name or description for existing Restaurant Menu Category (admin role)
        /// </summary>
        [Authorize(Policy = AuthorizationPolicies.AdminRolePolicy)]
        [HttpPut("menucategory/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MenuCategoryResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateMenuCategory(Guid id, MenuCategoryRequest request) // Careful to change that in the future if necessary
        {
            ErrorOr<MenuCategoryResult> updateMenuCategoryCommand = await _sender.Send(
                new UpdateMenuCategoryCommand(
                    id,
                    request.Name,
                    request.Description)
                );

            return updateMenuCategoryCommand.Match(
                updateCategoryResult => Ok(GetRestaurantMenuCategoryResponse(updateCategoryResult)),
                errors => Problem(errors));
        }

        /// <summary>
        /// Delete Restaurant Menu Category (admin role)
        /// </summary>
        [Authorize(Policy = AuthorizationPolicies.AdminRolePolicy)]
        [HttpDelete("menucategory/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteMenuCategory(Guid id)
        {
            ErrorOr<Unit> deleteMenuCategoryCommand = await _sender.Send(
                    new DeleteMenuCategoryCommand(id));

            return deleteMenuCategoryCommand.Match(
                deleteMenuCategoryResult => NoContent(),
                errors => Problem(errors));
        }

        /// <summary>
        /// Get informations about a specific Restaurant Menu Category like name and description
        /// </summary>
        [Authorize(Policy = AuthorizationPolicies.ClientOrAdminRolePolicy)]
        [HttpGet("menucategory/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MenuCategoryResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetRestaurantMenuCategoryById(Guid id)
        {
            ErrorOr<MenuCategoryResult> menuCategoryResult  = await _sender.Send(
                    new GetMenuCategoryByIdQuery(id));

            return menuCategoryResult.Match(
                categoryResult => Ok(GetRestaurantMenuCategoryResponse(categoryResult)),
                errors => Problem(errors));
        }

        /// <summary>
        /// Get all Restaurant Menu Categories available in Restaurant Simulation
        /// </summary>
        [Authorize(Policy = AuthorizationPolicies.ClientOrAdminRolePolicy)]
        [HttpGet("menucategories")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<MenuCategoryResponse>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetRestaurantMenuCategories()
        {
            ErrorOr<List<MenuCategoryResult>> menuCategoriesResult = await _sender.Send(
                    new GetMenuCategoriesQuery());

            return menuCategoriesResult.Match(
                menuCategoriesResult => Ok(GetRestaurantMenuCategoriesResponse(menuCategoriesResult)),
                errors => Problem(errors));
        }


        /// <summary>
        /// Get all Products from a  Restaurant Menu Category available in Restaurant Simulation
        /// </summary>
        [Authorize(Policy = AuthorizationPolicies.ClientOrAdminRolePolicy)]
        [HttpGet("menucategory/{id}/products")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ProductResult>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetProductsFromRestaurantMenuCategory(Guid id)
        {
            ErrorOr<List<ProductResult>> productsResult = await _sender.Send(
                    new GetProductsMenuCategoryByIdQuery(id));

            return productsResult.Match(
                productsResult => Ok(productsResult.Select(product => 
                    new ProductResponse(
                        product.Id, 
                        product.Price, 
                        product.Name, 
                        product.Description, 
                        product.isAvailable, 
                        product.CategoryId))),
                errors => Problem(errors));
        }

        private static MenuCategoryResponse GetRestaurantMenuCategoryResponse(MenuCategoryResult menuCategoryResult)
        {
            return new MenuCategoryResponse(
                            menuCategoryResult.Id,
                            menuCategoryResult.Name,
                            menuCategoryResult.Description);
        }

        private static List<MenuCategoryResponse> GetRestaurantMenuCategoriesResponse(List<MenuCategoryResult> menuCategoriesResult)
        {
            return menuCategoriesResult.Select(menuCategoryResult => 
                new MenuCategoryResponse(menuCategoryResult.Id, menuCategoryResult.Name, menuCategoryResult.Description)).ToList();
        }
    }
}
