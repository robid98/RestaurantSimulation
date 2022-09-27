using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantSimulation.Application.Restaurant.RestaurantMenuCategory.Commands.CreateMenuCategory;
using RestaurantSimulation.Application.Restaurant.RestaurantMenuCategory.Commands.DeleteMenuCategory;
using RestaurantSimulation.Application.Restaurant.RestaurantMenuCategory.Commands.UpdateMenuCategory;
using RestaurantSimulation.Application.Restaurant.RestaurantMenuCategory.Common;
using RestaurantSimulation.Contracts.Restaurant;
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
        [HttpPost("restaurant/menucategory")]
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
        [HttpPut("restaurant/menucategory/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MenuCategoryResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateMenuCategory(Guid id, MenuCategoryRequest request)
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
        [HttpDelete("restaurant/menucategory/{id}")]
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
        [HttpGet("restaurant/menucategory/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult GetRestaurantMenuCategoryById(Guid id)
        {
            return Ok();
        }

        /// <summary>
        /// Get all Restaurant Menu Categories available in Restaurant Simulation
        /// </summary>
        [Authorize(Policy = AuthorizationPolicies.ClientOrAdminRolePolicy)]
        [HttpGet("restaurant/menucategories")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult GetRestaurantMenuCategories()
        {
            return Ok();
        }


        /// <summary>
        /// Get all Products from a  Restaurant Menu Category available in Restaurant Simulation
        /// </summary>
        [Authorize(Policy = AuthorizationPolicies.ClientOrAdminRolePolicy)]
        [HttpGet("restaurant/menucategory/{id}/products")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult GetProductsFromRestaurantMenuCategory(Guid id)
        {
            return Ok();
        }

        private static MenuCategoryResponse GetRestaurantMenuCategoryResponse(MenuCategoryResult menuCategoryResult)
        {
            return new MenuCategoryResponse(
                            menuCategoryResult.Id,
                            menuCategoryResult.Name,
                            menuCategoryResult.Description);
        }
    }
}
