using RestaurantSimulation.Application.Authentication.Common;
using RestaurantSimulation.Contracts.Restaurant.MenuCategory;
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
            _integrationTestsSetup.AuthenticateAsync(RestaurantSimulationRoles.ClientRole, "test_mail@restaurant.com", _integrationTestsSetup.userSub);

            var responseGet = await _integrationTestsSetup.TestClient.GetAsync("/api/restaurant/menucategories/");

            responseGet.StatusCode.ShouldBe(HttpStatusCode.OK);

            var categories = await responseGet.Content.ReadFromJsonAsync<List<MenuCategoryResponse>>();

            categories?.Count.ShouldBe(RestaurantContextSeed.menuCategories.Count);
        }
    }
}
