using RestaurantSimulation.Application.Authentication.Common;
using RestaurantSimulation.Contracts.Authentication;
using Shouldly;
using System.Net;
using System.Net.Http.Json;

namespace RestaurantSimulation.Tests.IntegrationTests.Authentication
{
    public class AuthenticationControllerTests : IntegrationTests
    {
        [Fact]
        public async Task Should_Add_A_New_User()
        {
            AuthenticateAsync();

            var responsePost = await TestClient.PostAsJsonAsync("/api/auth/users/register", 
                new RegisterRequest(
                    FirstName: "Mirel",
                    LastName: "Doctorul", 
                    PhoneNumber: "0773823900",
                    Address: "Piatra Neamt"));

            responsePost.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Should_Get_A_List_Of_Users()
        {
            AuthenticateAsync();

            var responsePost = await TestClient.PostAsJsonAsync("/api/auth/users/register",
                new RegisterRequest(
                    FirstName: "Mirel",
                    LastName: "Doctorul",
                    PhoneNumber: "0773823900",
                    Address: "Piatra Neamt"));

            responsePost.StatusCode.ShouldBe(HttpStatusCode.OK);

            var responseGet = await TestClient.GetAsync("/api/auth/users/");

            responseGet.StatusCode.ShouldBe(HttpStatusCode.OK);

            var users = await responseGet.Content.ReadFromJsonAsync<List<AuthenticationResult>>();

            users?.Count.ShouldBe(1);
            users?[0].FirstName.ShouldBe("Mirel");
        }
    }
}
