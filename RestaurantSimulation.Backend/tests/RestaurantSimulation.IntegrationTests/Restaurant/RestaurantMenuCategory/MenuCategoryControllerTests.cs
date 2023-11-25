using System.Net;
using System.Net.Http.Json;
using RestaurantSimulation.Contracts.Restaurant.MenuCategory;
using RestaurantSimulation.Contracts.Restaurant.Product;
using RestaurantSimulation.Domain.Common.Roles;
using RestaurantSimulation.IntegrationTests.Helpers;
using Shouldly;

namespace RestaurantSimulation.IntegrationTests.Restaurant.RestaurantMenuCategory
{
    public class MenuCategoryControllerTests : CustomWebApplicationBase, IAsyncLifetime
    {
        public MenuCategoryControllerTests(CustomWebApplicationFactory<Program> factory) : base(factory)
        {

        }

        [Fact]
        public async Task GetAsync_ShouldGetAListOfMenuCategories()
        {
            // Arrange
            AuthenticateAsync(RestaurantSimulationRoles.AdminRole, "test_mail@restaurant.com", _userSub);

            await _httpClient.PostAsJsonAsync("/api/restaurant/menucategory",
                new MenuCategoryRequest("categorie de vara", "cea mai frumoasa categorie de vara"));

            // Act
            var responseGet = await _httpClient.GetAsync("/api/restaurant/menucategories/");

            // Assert
            responseGet.StatusCode.ShouldBe(HttpStatusCode.OK);

            var categories = await responseGet.Content.ReadFromJsonAsync<List<MenuCategoryResponse>>();

            categories.ShouldNotBeNull();
            categories.Count.ShouldBe(1);
        }


        [Fact]
        public async Task GetAsync_ShouldGetSpecificMenuCategoryById()
        {
            // Arrange
            AuthenticateAsync(RestaurantSimulationRoles.AdminRole, "test_mail@restaurant.com", _userSub);

            await _httpClient.PostAsJsonAsync("/api/restaurant/menucategory",
                new MenuCategoryRequest("categorie de vara", "cea mai frumoasa categorie de vara"));

            var menuCategories = await _httpClient.GetFromJsonAsync<List<MenuCategoryResponse>>("/api/restaurant/menucategories/");

            // Act
            var responseGet = await _httpClient.GetAsync($"/api/restaurant/menucategory/{menuCategories.First().Id}");

            // Assert
            responseGet.StatusCode.ShouldBe(HttpStatusCode.OK);

            MenuCategoryResponse category = await responseGet.Content.ReadFromJsonAsync<MenuCategoryResponse>();

            category?.Name.ShouldBe(menuCategories.First().Name);
            category?.Description.ShouldBe(menuCategories.First().Description);
        }

        [Fact]
        public async Task GetAsync_WhenCategoryIsNotFoundById_ShouldReturnNotFound()
        {
            // Arrange
            AuthenticateAsync(RestaurantSimulationRoles.ClientRole, "test_mail@restaurant.com", _userSub);

            // Act
            var responseGet = await _httpClient.GetAsync($"/api/restaurant/menucategory/{Guid.NewGuid()}");

            // Assert
            responseGet.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task PostAsJsonAsync_WhenNewCategoryWantToBeCreatedAndUserIsNotAdmin_ShouldReturnForbidden()
        {
            // Arrange
            AuthenticateAsync(RestaurantSimulationRoles.ClientRole, "test_mail@restaurant.com", _userSub);

            // Act
            var responsePost = await _httpClient.PostAsJsonAsync("/api/restaurant/menucategory",
                new MenuCategoryRequest("categorie de vara", "cea mai frumoasa categorie de vara"));

            // Assert
            responsePost.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task PostAsJsonAsync_WithValidDetails_ShouldCreateNewNenuCategory()
        {
            // Arrange
            AuthenticateAsync(RestaurantSimulationRoles.AdminRole, "test_mail@restaurant.com", _userSub);

            // Act
            var responsePost = await _httpClient.PostAsJsonAsync("/api/restaurant/menucategory",
                new MenuCategoryRequest("categorie de vara", "cea mai frumoasa categorie de vara"));


            MenuCategoryResponse category = await responsePost.Content.ReadFromJsonAsync<MenuCategoryResponse>();

            // Assert
            responsePost.StatusCode.ShouldBe(HttpStatusCode.OK);
            category?.Name.ShouldBe("categorie de vara");
            category?.Description.ShouldBe("cea mai frumoasa categorie de vara");
        }

        [Fact]
        public async Task PutAsJsonAsync_WhenACategoryWantToBeUpdatedAndUserIsNotAdmin_ShouldReturnForbidden()
        {
            // Arrange
            AuthenticateAsync(RestaurantSimulationRoles.ClientRole, "test_mail@restaurant.com", _userSub);

            // Act
            var responseput = await _httpClient.PutAsJsonAsync($"/api/restaurant/menucategory/{Guid.NewGuid()}",
                new MenuCategoryRequest("categorie de iarna", "cea mai frumoasa categorie de iarna"));

            // Assert
            responseput.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task GetAsync_IfCategoryIdDoesNotExistWhenGettingProductsFromCategory_ShouldReturnNoFound()
        {
            // Arrange
            AuthenticateAsync(RestaurantSimulationRoles.ClientRole, "test_mail@restaurant.com", _userSub);

            // Act
            var responseget = await _httpClient.GetAsync($"/api/restaurant/menucategory/{Guid.NewGuid()}/products");

            // Assert
            responseget.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetAsync_WhenCategoryDoesNotHaveProducts_ShouldReturnEmptyProductList()
        {
            // arrange
            AuthenticateAsync(RestaurantSimulationRoles.ClientRole, "test_mail@restaurant.com", _userSub);

            // act
            var responseGet = await _httpClient.GetAsync($"/api/restaurant/menucategory/{Guid.NewGuid()}/products");

            List<ProductResponse> products = await responseGet.Content.ReadFromJsonAsync<List<ProductResponse>>();

            // assert
            responseGet.StatusCode.ShouldBe(HttpStatusCode.OK);

            products?.Count.ShouldBe(0);
        }

        [Fact]
        public async Task PostAsJsonAsync_IfValidationsForNameWillFailWhenCreatingMenuCategory_ShouldReturnBadRequest()
        {
            // Arrange 
            AuthenticateAsync(RestaurantSimulationRoles.AdminRole, "test_mail@restaurant.com", _userSub);

            // Act
            var responsePost = await _httpClient.PostAsJsonAsync("/api/restaurant/menucategory",
                new MenuCategoryRequest("", "cea mai frumoasa categorie de vara"));

            // Assert
            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            // Act
            responsePost = await _httpClient.PostAsJsonAsync("/api/restaurant/menucategory",
                new MenuCategoryRequest("s", "cea mai frumoasa categorie de vara"));

            // Assert
            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            // Act
            responsePost = await _httpClient.PostAsJsonAsync("/api/restaurant/menucategory",
                new MenuCategoryRequest("%$%!ssadqweqdas", "cea mai frumoasa categorie de vara"));

            // Assert
            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task PostAsJsonAsync_IfValidationsForDescriptionWillFailWhenCreatingMenuCategory_ShouldReturnBadRequest()
        {
            // Arrange 
            AuthenticateAsync(RestaurantSimulationRoles.AdminRole, "test_mail@restaurant.com", _userSub);

            // Act
            var responsePost = await _httpClient.PostAsJsonAsync("/api/restaurant/menucategory",
                new MenuCategoryRequest("prajituri", "cea"));

            // Assert
            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            // Act
            responsePost = await _httpClient.PostAsJsonAsync("/api/restaurant/menucategory",
                new MenuCategoryRequest("prajituri", ""));

            // Assert
            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            // Act
            responsePost = await _httpClient.PostAsJsonAsync("/api/restaurant/menucategory",
                new MenuCategoryRequest("prajituri", "qweqe2!#!543"));

            // Assert
            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task PutAsJsonaAync_WhenUpdatingMenuCategoryIfValidationsForNameWillFail_ShouldReturnBadRequest()
        {
            // Arrange 
            AuthenticateAsync(RestaurantSimulationRoles.AdminRole, "test_mail@restaurant.com", _userSub);

            // Act
            var responsePost = await _httpClient.PutAsJsonAsync($"/api/restaurant/menucategory/{Guid.NewGuid()}",
                new MenuCategoryRequest("", "cea mai frumoasa categorie de vara"));

            // Assert
            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            // Act
            responsePost = await _httpClient.PutAsJsonAsync($"/api/restaurant/menucategory/{Guid.NewGuid()}",
                new MenuCategoryRequest("s", "cea mai frumoasa categorie de vara"));

            // Assert
            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            // Act
            responsePost = await _httpClient.PutAsJsonAsync($"/api/restaurant/menucategory/{Guid.NewGuid()}",
                new MenuCategoryRequest("%$%!ssadqweqdas", "cea mai frumoasa categorie de vara"));

            // Assert
            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task PutAsJsonAsyncShould_IfValidationsForDescriptionWillFailWhenUpdatingMenuCategory_ShouldReturnBadRequest()
        {
            // Arrange 
            AuthenticateAsync(RestaurantSimulationRoles.AdminRole, "test_mail@restaurant.com", _userSub);

            // Act
            var responsePost = await _httpClient.PutAsJsonAsync($"/api/restaurant/menucategory/{Guid.NewGuid()}",
                new MenuCategoryRequest("prajituri", "cea"));

            // Assert
            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            // Act
            responsePost = await _httpClient.PutAsJsonAsync($"/api/restaurant/menucategory/{Guid.NewGuid()}",
                new MenuCategoryRequest("prajituri", ""));

            // Assert
            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            // Act
            responsePost = await _httpClient.PutAsJsonAsync($"/api/restaurant/menucategory/{Guid.NewGuid()}",
                new MenuCategoryRequest("prajituri", "qweqe2!#!543"));

            // Assert
            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public async Task DisposeAsync()
        {
            await _respawner.ResetAsync(_connection);
        }
    }
}
