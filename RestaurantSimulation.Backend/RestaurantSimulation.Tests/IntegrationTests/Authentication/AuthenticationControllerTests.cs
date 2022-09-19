using RestaurantSimulation.Application.Authentication.Common;
using RestaurantSimulation.Contracts.Authentication;
using RestaurantSimulation.Domain.Common.Roles;
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
            await AuthenticateAndRegisterClientUser();
        }

        [Fact]
        public async Task Should_Get_A_List_Of_Users()
        {
            await AuthenticateAndRegisterAdminUser();

            var responseGet = await TestClient.GetAsync("/api/auth/users/");

            responseGet.StatusCode.ShouldBe(HttpStatusCode.OK);

            var users = await responseGet.Content.ReadFromJsonAsync<List<AuthenticationResult>>();

            users?.Count.ShouldBe(1);
            users?[0].FirstName.ShouldBe("Mirel");
        }

        [Fact]
        public async Task Should_Get_User_By_Access_Token()
        {
            await AuthenticateAndRegisterClientUser();

            var responseGet = await TestClient.GetAsync("/api/auth/user");

            responseGet.StatusCode.ShouldBe(HttpStatusCode.OK);

            var user = await responseGet.Content.ReadFromJsonAsync<AuthenticationResult>();

            user?.FirstName.ShouldBe("Mirel");
            user?.PhoneNumber.ShouldBe("0773823901");
        }

        [Fact]
        public async Task Should_Return_Not_Found_When_User_Is_Not_Registered_Getting_By_Access_Token()
        {
            AuthenticateAsync(RestaurantSimulationRoles.ClientRole, "test_mail@restaurant.com");

            var responseGet = await TestClient.GetAsync("/api/auth/users/accesstoken");

            responseGet.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_Return_Not_Found_If_A_User_With_Specified_Id_Not_Registered()
        {
            AuthenticateAsync(RestaurantSimulationRoles.AdminRole, "test_mail@restaurant.com");

            var responseGet = await TestClient.GetAsync($"/api/auth/users/{Guid.NewGuid()}");

            responseGet.StatusCode.ShouldBe(HttpStatusCode.NotFound);

        }

        [Fact]
        public async Task Should_Return_User_Registered_With_Specified_Id()
        {
            await AuthenticateAndRegisterAdminUser();

            var responseGet = await TestClient.GetAsync("/api/auth/users/");

            var users = await responseGet.Content.ReadFromJsonAsync<List<AuthenticationResult>>();

            users?.Count.ShouldBe(1);
            users?[0].FirstName.ShouldBe("Mirel");

            var responseGetUserById = await TestClient.GetAsync($"/api/auth/user/{users?[0].Id}");

            responseGetUserById.StatusCode.ShouldBe(HttpStatusCode.OK);

            var user = await responseGetUserById.Content.ReadFromJsonAsync<AuthenticationResult>();

            user?.FirstName.ShouldBe("Mirel");
            user?.PhoneNumber.ShouldBe("0773823901");

        }

        [Fact]
        public async Task Should_Update_Existing_User()
        {
            await AuthenticateAndRegisterClientUser();

            var responseHttpPutUser = await TestClient.PutAsJsonAsync("/api/auth/users",
                CreateRegisterUserRequest("Mirel", "Danilu", "0773823902", "Piatra Neamt, Boy"));

            responseHttpPutUser.StatusCode.ShouldBe(HttpStatusCode.OK);

            var user = await responseHttpPutUser.Content.ReadFromJsonAsync<AuthenticationResult>();

            user?.FirstName.ShouldBe("Mirel");
            user?.LastName.ShouldBe("Danilu");
            user?.Email.ShouldBe("test_mail@restaurant.com");
            user?.PhoneNumber.ShouldBe("0773823902");
            user?.Address.ShouldBe("Piatra Neamt, Boy");

            var responseGetUser = await TestClient.GetAsync($"/api/auth/user");

            responseGetUser.StatusCode.ShouldBe(HttpStatusCode.OK);

            var userById = await responseGetUser.Content.ReadFromJsonAsync<AuthenticationResult>();

            userById?.FirstName.ShouldBe("Mirel");
            userById?.LastName.ShouldBe("Danilu");
            userById?.Email.ShouldBe("test_mail@restaurant.com");
            userById?.PhoneNumber.ShouldBe("0773823902");
            userById?.Address.ShouldBe("Piatra Neamt, Boy");
        }

        [Fact]
        public async Task Should_Return_404_If_User_Dont_Exist_And_Want_To_Be_Updated()
        {
            AuthenticateAsync(RestaurantSimulationRoles.ClientRole, "test_mail@restaurant.com");

            var responseHttpPutUser = await TestClient.PutAsJsonAsync("/api/auth/users",
                CreateRegisterUserRequest("Mirel", "Danilu", "0773823902", "Piatra Neamt, Boy"));

            responseHttpPutUser.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }


        [Fact]
        public async Task Should_Return_Bad_Request_If_Validations_For_First_Name_Will_Fail()
        {
            AuthenticateAsync(RestaurantSimulationRoles.ClientRole, "test_mail@restaurant.com");

            var responsePost = await TestClient.PostAsJsonAsync("/api/auth/users",
                CreateRegisterUserRequest("", "Doctorul", "0773823901", "Piatra Neamt"));

            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            responsePost = await TestClient.PostAsJsonAsync("/api/auth/users",
                CreateRegisterUserRequest("12311", "Doctorul", "0773823901", "Piatra Neamt"));

            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            responsePost = await TestClient.PostAsJsonAsync("/api/auth/users",
                CreateRegisterUserRequest("Ro", "Doctorul", "0773823901", "Piatra Neamt"));

            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_Return_Bad_Request_If_Validations_For_Last_Name_Will_Fail()
        {
            AuthenticateAsync(RestaurantSimulationRoles.ClientRole, "test_mail@restaurant.com");

            var responsePost = await TestClient.PostAsJsonAsync("/api/auth/users",
                CreateRegisterUserRequest("Mirel", "", "0773823901", "Piatra Neamt"));

            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            responsePost = await TestClient.PostAsJsonAsync("/api/auth/users",
                CreateRegisterUserRequest("Mirel", "12311", "0773823901", "Piatra Neamt"));

            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            responsePost = await TestClient.PostAsJsonAsync("/api/auth/users",
                CreateRegisterUserRequest("Mirel", "Do", "0773823901", "Piatra Neamt"));

            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_Return_Bad_Request_If_Validations_For_PhoneNumber_Will_Fail()
        {
            AuthenticateAsync(RestaurantSimulationRoles.ClientRole, "test_mail@restaurant.com");

            var responsePost = await TestClient.PostAsJsonAsync("/api/auth/users",
                CreateRegisterUserRequest("Mirel", "Dorel", "07738239011", "Piatra Neamt"));

            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            responsePost = await TestClient.PostAsJsonAsync("/api/auth/users",
                CreateRegisterUserRequest("Mirel", "Dorel", "0773823901a", "Piatra Neamt"));

            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            responsePost = await TestClient.PostAsJsonAsync("/api/auth/users",
                CreateRegisterUserRequest("Mirel", "Dorel", "0773823^01a", "Piatra Neamt"));

            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            responsePost = await TestClient.PostAsJsonAsync("/api/auth/users",
                CreateRegisterUserRequest("Mirel", "Dorel", "aaa", "Piatra Neamt"));

            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_Return_Bad_Request_If_Validations_For_Address_Will_Fail()
        {
            AuthenticateAsync(RestaurantSimulationRoles.ClientRole, "test_mail@restaurant.com");

            var responsePost = await TestClient.PostAsJsonAsync("/api/auth/users",
                CreateRegisterUserRequest("Mirel", "Dorel", "0773823901", "heh^^"));

            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            responsePost = await TestClient.PostAsJsonAsync("/api/auth/users",
                CreateRegisterUserRequest("Mirel", "Dorel", "0773823901", "Pp"));

            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }


        /**************/
        /***HELPERS****/
        /*************/
        public async Task AuthenticateAndRegisterClientUser()
        {
            AuthenticateAsync(RestaurantSimulationRoles.ClientRole, "test_mail@restaurant.com");

            var responsePost = await TestClient.PostAsJsonAsync("/api/auth/users",
                CreateRegisterUserRequest("Mirel", "Doctorul", "0773823901", "Piatra Neamt"));

            responsePost.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        public async Task AuthenticateAndRegisterAdminUser()
        {
            AuthenticateAsync(RestaurantSimulationRoles.AdminRole, "test_mail@restaurant.com");

            var responsePost = await TestClient.PostAsJsonAsync("/api/auth/users",
                CreateRegisterUserRequest("Mirel", "Doctorul", "0773823901", "Piatra Neamt"));

            responsePost.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        private RegisterUserRequest CreateRegisterUserRequest(string firstName, string lastName, string phoneNumber, string address)
        {
            return new RegisterUserRequest(
                FirstName: firstName,
                LastName: lastName,
                PhoneNumber: phoneNumber,
                Address: address);
        }
    }
}
