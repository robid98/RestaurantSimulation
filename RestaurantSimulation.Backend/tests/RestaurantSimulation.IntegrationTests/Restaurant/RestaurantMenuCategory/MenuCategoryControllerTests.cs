using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using Microsoft.EntityFrameworkCore;
using RestaurantSimulation.Contracts.Restaurant.MenuCategory;
using RestaurantSimulation.Domain.Common.Roles;
using RestaurantSimulation.IntegrationTests.Helpers;
using Shouldly;

namespace RestaurantSimulation.IntegrationTests.Restaurant.RestaurantMenuCategory
{
    public class MenuCategoryControllerTests : CustomWebApplicationBase, IAsyncLifetime
    {
        private string _baseApiPath = "/api/restaurant";

        public MenuCategoryControllerTests(CustomWebApplicationFactory<Program> factory) : base(factory)
        {

        }

        [Fact]
        public async Task GetAsync_ShouldGetAllMenuCategoriesFromRestaurantSimulation()
        {
            // Arrange
            var menuCategoryRequest = _fixture.Build<MenuCategoryRequest>()
                .With(x => x.Name, "Test Category")
                .With(x => x.Description, "A beautiful food category in this world")
                .Create();

            AuthenticateAsync(RestaurantSimulationRoles.AdminRole, "test_mail@restaurant.com", _userSub);

            await _httpClient.PostAsJsonAsync($"{_baseApiPath}/menucategory", menuCategoryRequest);

            // Act
            var responseGet = await _httpClient.GetAsync($"{_baseApiPath}/menucategories/");

            // Assert
            responseGet.StatusCode.ShouldBe(HttpStatusCode.OK);

            var categories = await responseGet.Content.ReadFromJsonAsync<List<MenuCategoryResponse>>();

            categories.ShouldNotBeNull();
            categories.Where(x => x.Name == menuCategoryRequest.Name).Count().ShouldBe(1);
        }


        [Fact]
        public async Task GetAsync_WithValidId_ShouldGetSpecificMenuCategoryForTheProvidedId()
        {
            // Arrange
            AuthenticateAsync(RestaurantSimulationRoles.AdminRole, "test_mail@restaurant.com", _userSub);

            await _httpClient.PostAsJsonAsync($"{_baseApiPath}/menucategory",
                new MenuCategoryRequest("categorie de vara", "cea mai frumoasa categorie de vara"));

            var menuCategories = await _httpClient.GetFromJsonAsync<List<MenuCategoryResponse>>($"{_baseApiPath}/menucategories/");

            // Act
            var responseGet = await _httpClient.GetAsync($"{_baseApiPath}/menucategory/{menuCategories.First().Id}");

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
            var responseGet = await _httpClient.GetAsync($"{_baseApiPath}/menucategory/{Guid.NewGuid()}");

            // Assert
            responseGet.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task PostAsJsonAsync_WhenNewCategoryWantToBeCreatedAndUserIsNotAdmin_ShouldReturnForbidden()
        {
            // Arrange
            AuthenticateAsync(RestaurantSimulationRoles.ClientRole, "test_mail@restaurant.com", _userSub);

            // Act
            var responsePost = await _httpClient.PostAsJsonAsync($"{_baseApiPath}/menucategory",
                new MenuCategoryRequest("categorie de vara", "cea mai frumoasa categorie de vara"));

            // Assert
            responsePost.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task PostAsJsonAsync_WithValidDetails_ShouldCreateNewNenuCategoryInRestaurantSimulation()
        {
            // Arrange
            var menuCategoryRequest = _fixture.Build<MenuCategoryRequest>()
                .With(x => x.Name, "Something random at this moment")
                .With(x => x.Description, "Something Random Description Best Category")
                .Create();

            AuthenticateAsync(RestaurantSimulationRoles.AdminRole, "test_mail@restaurant.com", _userSub);

            // Act
            var responsePost = await _httpClient.PostAsJsonAsync($"{_baseApiPath}/menucategory", menuCategoryRequest);

            MenuCategoryResponse category = await responsePost.Content.ReadFromJsonAsync<MenuCategoryResponse>();

            // Assert
            responsePost.StatusCode.ShouldBe(HttpStatusCode.OK);
            category?.Name.ShouldBe(menuCategoryRequest.Name);
            category?.Description.ShouldBe(menuCategoryRequest.Description);
        }

        [Fact]
        public async Task PutAsJsonAsync_WhenACategoryWantToBeUpdatedAndUserIsNotAdmin_ShouldReturnForbidden()
        {
            // Arrange
            AuthenticateAsync(RestaurantSimulationRoles.ClientRole, "test_mail@restaurant.com", _userSub);

            // Act
            var responseput = await _httpClient.PutAsJsonAsync($"{_baseApiPath}/menucategory/{Guid.NewGuid()}",
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
            var responseget = await _httpClient.GetAsync($"{_baseApiPath}/menucategory/{Guid.NewGuid()}/products");

            // Assert
            responseget.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task PostAsJsonAsync_IfValidationsForNameWillFailWhenCreatingMenuCategory_ShouldReturnBadRequest()
        {
            // Arrange 
            AuthenticateAsync(RestaurantSimulationRoles.AdminRole, "test_mail@restaurant.com", _userSub);

            // Act
            var responsePost = await _httpClient.PostAsJsonAsync($"{_baseApiPath}/menucategory",
                new MenuCategoryRequest("", "cea mai frumoasa categorie de vara"));

            // Assert
            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            // Act
            responsePost = await _httpClient.PostAsJsonAsync($"{_baseApiPath}/menucategory",
                new MenuCategoryRequest("s", "cea mai frumoasa categorie de vara"));

            // Assert
            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            // Act
            responsePost = await _httpClient.PostAsJsonAsync($"{_baseApiPath}/menucategory",
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
            var responsePost = await _httpClient.PostAsJsonAsync($"{_baseApiPath}/menucategory",
                new MenuCategoryRequest("prajituri", "cea"));

            // Assert
            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            // Act
            responsePost = await _httpClient.PostAsJsonAsync($"{_baseApiPath}/menucategory",
                new MenuCategoryRequest("prajituri", ""));

            // Assert
            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            // Act
            responsePost = await _httpClient.PostAsJsonAsync($"{_baseApiPath}/menucategory",
                new MenuCategoryRequest("prajituri", "qweqe2!#!543"));

            // Assert
            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task PutAsJsonAsync_WhenUpdatingMenuCategoryIfValidationsForNameWillFail_ShouldReturnBadRequest()
        {
            // Arrange 
            AuthenticateAsync(RestaurantSimulationRoles.AdminRole, "test_mail@restaurant.com", _userSub);

            // Act
            var responsePost = await _httpClient.PutAsJsonAsync($"{_baseApiPath}/menucategory/{Guid.NewGuid()}",
                new MenuCategoryRequest("", "cea mai frumoasa categorie de vara"));

            // Assert
            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            // Act
            responsePost = await _httpClient.PutAsJsonAsync($"{_baseApiPath}/menucategory/{Guid.NewGuid()}",
                new MenuCategoryRequest("s", "cea mai frumoasa categorie de vara"));

            // Assert
            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            // Act
            responsePost = await _httpClient.PutAsJsonAsync($"{_baseApiPath}/menucategory/{Guid.NewGuid()}",
                new MenuCategoryRequest("%$%!ssadqweqdas", "cea mai frumoasa categorie de vara"));

            // Assert
            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task PutAsJsonAsync_IfValidationsForDescriptionWillFailWhenUpdatingMenuCategory_ShouldReturnBadRequest()
        {
            // Arrange 
            AuthenticateAsync(RestaurantSimulationRoles.AdminRole, "test_mail@restaurant.com", _userSub);

            // Act
            var responsePost = await _httpClient.PutAsJsonAsync($"{_baseApiPath}/menucategory/{Guid.NewGuid()}",
                new MenuCategoryRequest("prajituri", "cea"));

            // Assert
            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            // Act
            responsePost = await _httpClient.PutAsJsonAsync($"{_baseApiPath}/menucategory/{Guid.NewGuid()}",
                new MenuCategoryRequest("prajituri", ""));

            // Assert
            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            // Act
            responsePost = await _httpClient.PutAsJsonAsync($"{_baseApiPath}/menucategory/{Guid.NewGuid()}",
                new MenuCategoryRequest("prajituri", "qweqe2!#!543"));

            // Assert
            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task DeleteAsync_WhenACategoryNeedsToBeDeletedAndUserIsNotAdmin_ShouldReturnForbidden()
        {
            // Arrange
            AuthenticateAsync(RestaurantSimulationRoles.ClientRole, "test_mail@restaurant.com", _userSub);

            // Act
            var responseDelete = await _httpClient.DeleteAsync($"{_baseApiPath}/menucategory/{"694d6ed1-4ef5-4539-926d-c459c2ba1b39"}");

            // Assert
            responseDelete.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task DeleteAsync_WhenACategoryNeedsToBeDeletedAndUserIsAdmin_ShouldDeleteTheCategory()
        {
            // Arrange
            AuthenticateAsync(RestaurantSimulationRoles.AdminRole, "test_mail@restaurant.com", _userSub);

            // Act
            var responseDelete = await _httpClient.DeleteAsync($"{_baseApiPath}/menucategory/{"694d6ed1-4ef5-4539-926d-c459c2ba1b39"}"); // Id is seeded

            // Assert
            responseDelete.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task DeleteAsync_WhenIsNotFoundAndUserIsAdmin_ShouldReturnNotFound()
        {
            // Arrange
            AuthenticateAsync(RestaurantSimulationRoles.AdminRole, "test_mail@restaurant.com", _userSub);

            // Act
            var responseDelete = await _httpClient.DeleteAsync($"{_baseApiPath}/menucategory/{Guid.NewGuid()}");

            // Assert
            responseDelete.StatusCode.ShouldBe(HttpStatusCode.NotFound);
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
