using RestaurantSimulation.Application.Authentication.Common;
using RestaurantSimulation.Contracts.Restaurant.MenuCategory;
using RestaurantSimulation.Contracts.Restaurant.Product;
using RestaurantSimulation.Domain.Common.Roles;
using Shouldly;
using System.Net;
using System.Net.Http.Json;
using Xunit.Extensions.Ordering;

namespace RestaurantSimulation.IntegrationTests.Restaurant.RestaurantMenuCategory
{
    public class MenuCategoryControllerTests : IClassFixture<IntegrationTestsSetup>
    {
        private readonly IntegrationTestsSetup _integrationTestsSetup;

        public MenuCategoryControllerTests(IntegrationTestsSetup integrationTestsSetup)
        {
            _integrationTestsSetup = integrationTestsSetup;
        }

        [Fact, Order(1)]
        public async Task Should_Get_A_List_Of_Menu_Categories()
        {
            // arrange
            _integrationTestsSetup.AuthenticateAsync(RestaurantSimulationRoles.ClientRole, "test_mail@restaurant.com", _integrationTestsSetup.userSub);

            // act
            var responseGet = await _integrationTestsSetup.TestClient.GetAsync("/api/restaurant/menucategories/");

            // assert
            responseGet.StatusCode.ShouldBe(HttpStatusCode.OK);

            var categories = await responseGet.Content.ReadFromJsonAsync<List<MenuCategoryResponse>>();

            categories?.Count.ShouldBe(RestaurantContextSeed.menuCategories.Count);
        }


        [Fact, Order(2)]
        public async Task Should_Get_Specific_Menu_Category_By_Id()
        {
            // arrange
            _integrationTestsSetup.AuthenticateAsync(RestaurantSimulationRoles.ClientRole, "test_mail@restaurant.com", _integrationTestsSetup.userSub);

            // act
            var responseGet = await _integrationTestsSetup.TestClient.GetAsync($"/api/restaurant/menucategory/{RestaurantContextSeed.categoriesGuid[0]}");

            // assert
            responseGet.StatusCode.ShouldBe(HttpStatusCode.OK);

            MenuCategoryResponse category = await responseGet.Content.ReadFromJsonAsync<MenuCategoryResponse>();

            category?.Name.ShouldBe(RestaurantContextSeed.menuCategories[0].Name);
            category?.Description.ShouldBe(RestaurantContextSeed.menuCategories[0].Description);
        }

        [Fact, Order(3)]
        public async Task Should_Return_Not_Found_If_Category_Is_Not_Found_By_Id()
        {
            // arrange
            _integrationTestsSetup.AuthenticateAsync(RestaurantSimulationRoles.ClientRole, "test_mail@restaurant.com", _integrationTestsSetup.userSub);

            // act
            var responseGet = await _integrationTestsSetup.TestClient.GetAsync($"/api/restaurant/menucategory/{Guid.NewGuid()}");

            // assert
            responseGet.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact, Order(4)]
        public async Task Should_Return_Forbidden_If_New_Category_Want_To_Be_Created_And_User_Is_Not_Admin()
        {
            // arrange
            _integrationTestsSetup.AuthenticateAsync(RestaurantSimulationRoles.ClientRole, "test_mail@restaurant.com", _integrationTestsSetup.userSub);

            // act
            var responsePost = await _integrationTestsSetup.TestClient.PostAsJsonAsync("/api/restaurant/menucategory",
                new MenuCategoryRequest("Categorie de vara", "Cea mai frumoasa categorie de vara"));

            // assert
            responsePost.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
        }

        [Fact, Order(5)]
        public async Task Should_Create_New_Menu_Category()
        {
            // arrange
            _integrationTestsSetup.AuthenticateAsync(RestaurantSimulationRoles.AdminRole, "dtest_mail@restaurant.com", _integrationTestsSetup.userSub);

            // act
            var responsePost = await _integrationTestsSetup.TestClient.PostAsJsonAsync("/api/restaurant/menucategory",
                new MenuCategoryRequest("Categorie de vara", "Cea mai frumoasa categorie de vara"));


            MenuCategoryResponse category = await responsePost.Content.ReadFromJsonAsync<MenuCategoryResponse>();

            // assert
            responsePost.StatusCode.ShouldBe(HttpStatusCode.OK);
            category?.Name.ShouldBe("Categorie de vara");
            category?.Description.ShouldBe("Cea mai frumoasa categorie de vara");
        }

        [Fact, Order(6)]
        public async Task Should_Return_Forbidden_If_A_Category_Want_To_Be_Updated_And_User_Is_Not_Admin()
        {
            // arrange
            _integrationTestsSetup.AuthenticateAsync(RestaurantSimulationRoles.ClientRole, "test_mail@restaurant.com", _integrationTestsSetup.userSub);

            // act
            var responsePut = await _integrationTestsSetup.TestClient.PutAsJsonAsync($"/api/restaurant/menucategory/{RestaurantContextSeed.categoriesGuid[0]}",
                new MenuCategoryRequest("Categorie de iarna", "Cea mai frumoasa categorie de iarna"));

            // assert
            responsePut.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
        }

        [Fact, Order(7)]
        public async Task Should_Update_Existing_Category_And_After_That_Conflict_If_Updating_With_Same_Name()
        {
            // arrange
            _integrationTestsSetup.AuthenticateAsync(RestaurantSimulationRoles.AdminRole, "test_mail@restaurant.com", _integrationTestsSetup.userSub);

            // act
            var responsePut = await _integrationTestsSetup.TestClient.PutAsJsonAsync($"/api/restaurant/menucategory/{RestaurantContextSeed.categoriesGuid[0]}",
                new MenuCategoryRequest("Categorie de iarna", "Cea mai frumoasa categorie de iarna"));

            // assert
            responsePut.StatusCode.ShouldBe(HttpStatusCode.OK);

            // act
            var responseGet = await _integrationTestsSetup.TestClient.GetAsync($"/api/restaurant/menucategory/{RestaurantContextSeed.categoriesGuid[0]}");

            // assert
            responseGet.StatusCode.ShouldBe(HttpStatusCode.OK);

            MenuCategoryResponse category = await responseGet.Content.ReadFromJsonAsync<MenuCategoryResponse>();

            category?.Name.ShouldBe("Categorie de iarna");
            category?.Description.ShouldBe("Cea mai frumoasa categorie de iarna");

            responsePut = await _integrationTestsSetup.TestClient.PutAsJsonAsync($"/api/restaurant/menucategory/{RestaurantContextSeed.categoriesGuid[0]}",
                new MenuCategoryRequest("Categorie de iarna", "Cea mai frumoasa categorie de iarna"));

            // assert
            responsePut.StatusCode.ShouldBe(HttpStatusCode.Conflict);
        }


        [Fact, Order(8)]
        public async Task Should_Return_Forbidden_If_A_Category_Want_To_Be_Deleted_And_User_Is_Not_Admin()
        {
            // arrange
            _integrationTestsSetup.AuthenticateAsync(RestaurantSimulationRoles.ClientRole, "test_mail@restaurant.com", _integrationTestsSetup.userSub);

            // act
            var responseDelete = await _integrationTestsSetup.TestClient.DeleteAsync($"/api/restaurant/menucategory/{RestaurantContextSeed.categoriesGuid[0]}");

            // assert
            responseDelete.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
        }

        [Fact, Order(9)]
        public async Task Should_Delete_A_Specific_Category_By_Id()
        {
            // arrange
            _integrationTestsSetup.AuthenticateAsync(RestaurantSimulationRoles.AdminRole, "test_mail@restaurant.com", _integrationTestsSetup.userSub);

            // act
            var responseDelete = await _integrationTestsSetup.TestClient.DeleteAsync($"/api/restaurant/menucategory/{RestaurantContextSeed.categoriesGuid[0]}");

            // assert
            responseDelete.StatusCode.ShouldBe(HttpStatusCode.NoContent);

            // act
            var responseGet = await _integrationTestsSetup.TestClient.GetAsync($"/api/restaurant/menucategory/{RestaurantContextSeed.categoriesGuid[0]}");

            // assert
            responseGet.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact, Order(10)]
        public async Task Should_Get_Products_From_Category_By_Id()
        {
            // arrange
            _integrationTestsSetup.AuthenticateAsync(RestaurantSimulationRoles.ClientRole, "test_mail@restaurant.com", _integrationTestsSetup.userSub);

            // act
            var responseGet = await _integrationTestsSetup.TestClient.GetAsync($"/api/restaurant/menucategory/{RestaurantContextSeed.categoriesGuid[1]}/products");

            List<ProductResponse> products = await responseGet.Content.ReadFromJsonAsync<List<ProductResponse>>();

            // assert
            responseGet.StatusCode.ShouldBe(HttpStatusCode.OK);

            products?.Count.ShouldBe(2);

            List<ProductResponse> sortedProducts = products?.OrderBy(o => o.Price).ToList();

            sortedProducts?[0].Price.ShouldBe(10.05);
            sortedProducts?[0].Description.ShouldBe("Carne, cartofi");

            sortedProducts?[1].Price.ShouldBe(11.50);
            sortedProducts?[1].Description.ShouldBe("Inghetata cu de toate");
        }

        [Fact, Order(11)]
        public async Task Should_Return_No_Found_If_Category_Id_Does_Not_Exist_When_Getting_Products_From_Category()
        {
            // arrange
            _integrationTestsSetup.AuthenticateAsync(RestaurantSimulationRoles.ClientRole, "test_mail@restaurant.com", _integrationTestsSetup.userSub);

            // act
            var responseGet = await _integrationTestsSetup.TestClient.GetAsync($"/api/restaurant/menucategory/{Guid.NewGuid()}/products");

            // assert
            responseGet.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact, Order(12)]
        public async Task Should_Return_Empty_Product_List_If_Category_Does_Not_Have_Products()
        {
            // arrange
            _integrationTestsSetup.AuthenticateAsync(RestaurantSimulationRoles.ClientRole, "test_mail@restaurant.com", _integrationTestsSetup.userSub);

            // act
            var responseGet = await _integrationTestsSetup.TestClient.GetAsync($"/api/restaurant/menucategory/{RestaurantContextSeed.categoriesGuid[2]}/products");

            List<ProductResponse> products = await responseGet.Content.ReadFromJsonAsync<List<ProductResponse>>();

            // assert
            responseGet.StatusCode.ShouldBe(HttpStatusCode.OK);

            products?.Count.ShouldBe(0);
        }

        [Fact]
        public async Task Should_Return_Bad_Request_If_Validations_For_Name_Will_Fail_When_Creating_Menu_Category()
        {
            // arrange 
            _integrationTestsSetup.AuthenticateAsync(RestaurantSimulationRoles.AdminRole, "test_mail@restaurant.com", _integrationTestsSetup.userSub);

            // act
            var responsePost = await _integrationTestsSetup.TestClient.PostAsJsonAsync("/api/restaurant/menucategory",
                new MenuCategoryRequest("", "Cea mai frumoasa categorie de vara"));

            // assert
            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            // act
            responsePost = await _integrationTestsSetup.TestClient.PostAsJsonAsync("/api/restaurant/menucategory",
                new MenuCategoryRequest("s", "Cea mai frumoasa categorie de vara"));

            // assert
            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            // act
            responsePost = await _integrationTestsSetup.TestClient.PostAsJsonAsync("/api/restaurant/menucategory",
                new MenuCategoryRequest("%$%!ssadqweqdas", "Cea mai frumoasa categorie de vara"));

            // assert
            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_Return_Bad_Request_If_Validations_For_Description_Will_Fail_When_Creating_Menu_Category()
        {
            // arrange 
            _integrationTestsSetup.AuthenticateAsync(RestaurantSimulationRoles.AdminRole, "test_mail@restaurant.com", _integrationTestsSetup.userSub);

            // act
            var responsePost = await _integrationTestsSetup.TestClient.PostAsJsonAsync("/api/restaurant/menucategory",
                new MenuCategoryRequest("Prajituri", "Cea"));

            // assert
            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            // act
            responsePost = await _integrationTestsSetup.TestClient.PostAsJsonAsync("/api/restaurant/menucategory",
                new MenuCategoryRequest("Prajituri", ""));

            // assert
            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            // act
            responsePost = await _integrationTestsSetup.TestClient.PostAsJsonAsync("/api/restaurant/menucategory",
                new MenuCategoryRequest("Prajituri", "qweqe2!#!543"));

            // assert
            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_Return_Bad_Request_If_Validations_For_Name_Will_Fail_When_Updating_Menu_Category()
        {
            // arrange 
            _integrationTestsSetup.AuthenticateAsync(RestaurantSimulationRoles.AdminRole, "test_mail@restaurant.com", _integrationTestsSetup.userSub);

            // act
            var responsePost = await _integrationTestsSetup.TestClient.PutAsJsonAsync($"/api/restaurant/menucategory/{Guid.NewGuid()}",
                new MenuCategoryRequest("", "Cea mai frumoasa categorie de vara"));

            // assert
            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            // act
            responsePost = await _integrationTestsSetup.TestClient.PutAsJsonAsync($"/api/restaurant/menucategory/{Guid.NewGuid()}",
                new MenuCategoryRequest("s", "Cea mai frumoasa categorie de vara"));

            // assert
            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            // act
            responsePost = await _integrationTestsSetup.TestClient.PutAsJsonAsync($"/api/restaurant/menucategory/{Guid.NewGuid()}",
                new MenuCategoryRequest("%$%!ssadqweqdas", "Cea mai frumoasa categorie de vara"));

            // assert
            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_Return_Bad_Request_If_Validations_For_Description_Will_Fail_When_Updating_Menu_Category()
        {
            // arrange 
            _integrationTestsSetup.AuthenticateAsync(RestaurantSimulationRoles.AdminRole, "test_mail@restaurant.com", _integrationTestsSetup.userSub);

            // act
            var responsePost = await _integrationTestsSetup.TestClient.PutAsJsonAsync($"/api/restaurant/menucategory/{Guid.NewGuid()}",
                new MenuCategoryRequest("Prajituri", "Cea"));

            // assert
            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            // act
            responsePost = await _integrationTestsSetup.TestClient.PutAsJsonAsync($"/api/restaurant/menucategory/{Guid.NewGuid()}",
                new MenuCategoryRequest("Prajituri", ""));

            // assert
            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            // act
            responsePost = await _integrationTestsSetup.TestClient.PutAsJsonAsync($"/api/restaurant/menucategory/{Guid.NewGuid()}",
                new MenuCategoryRequest("Prajituri", "qweqe2!#!543"));

            // assert
            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }
    }
}
