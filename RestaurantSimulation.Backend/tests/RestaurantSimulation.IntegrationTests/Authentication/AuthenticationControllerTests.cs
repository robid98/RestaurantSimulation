using RestaurantSimulation.Application.Authentication.Common;
using RestaurantSimulation.Contracts.Authentication;
using RestaurantSimulation.Domain.Common.Roles;
using Shouldly;
using System.Net;
using System.Net.Http.Json;
using Xunit.Extensions.Ordering;

namespace RestaurantSimulation.IntegrationTests.Authentication
{
    public class AuthenticationControllerTests : IClassFixture<IntegrationTestsSetup>
    {
        private readonly IntegrationTestsSetup _integrationTestsSetup;

        private RegisterUserRequest userRequest = new RegisterUserRequest(
                "Mirel", "Doctorul", "0773823901", "Piatra Neamt"
            );
            
        public AuthenticationControllerTests(IntegrationTestsSetup integrationTestsSetup)
        {
            _integrationTestsSetup = integrationTestsSetup;
        }

        [Fact, Order(1)]
        public async Task Should_Add_A_New_User()
        {
            _integrationTestsSetup.AuthenticateAsync(RestaurantSimulationRoles.AdminRole, "test_mail@restaurant.com", _integrationTestsSetup.userSub);

            var responsePost = await _integrationTestsSetup.TestClient.PostAsJsonAsync("/api/auth/user",
                CreateRegisterUserRequest(userRequest.FirstName, userRequest.LastName, userRequest.PhoneNumber, userRequest.Address));

            responsePost.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Fact, Order(2)]
        public async Task Should_Get_A_List_Of_Users()
        {
            _integrationTestsSetup.AuthenticateAsync(RestaurantSimulationRoles.AdminRole, "test_mail@restaurant.com", _integrationTestsSetup.userSub);

            var responseGet = await _integrationTestsSetup.TestClient.GetAsync("/api/auth/users/");

            responseGet.StatusCode.ShouldBe(HttpStatusCode.OK);

            var users = await responseGet.Content.ReadFromJsonAsync<List<AuthenticationResponse>>();

            users?.Count.ShouldBe(RestaurantContextSeed.users.Count + 1);
            users?[RestaurantContextSeed.users.Count].FirstName.ShouldBe(userRequest.FirstName);
        }

        [Fact, Order(3)]
        public async Task Should_Get_User_By_Access_Token()
        {
            _integrationTestsSetup.AuthenticateAsync(RestaurantSimulationRoles.AdminRole, "test_mail@restaurant.com", _integrationTestsSetup.userSub);

            var responseGet = await _integrationTestsSetup.TestClient.GetAsync("/api/auth/user");

            responseGet.StatusCode.ShouldBe(HttpStatusCode.OK);

            var user = await responseGet.Content.ReadFromJsonAsync<AuthenticationResponse>();

            user?.FirstName.ShouldBe(userRequest.FirstName);
            user?.PhoneNumber.ShouldBe(userRequest.PhoneNumber);
        }

        [Fact, Order(4)]
        public async Task Should_Return_Not_Found_When_User_Is_Not_Registered_Getting_By_Access_Token()
        {
            _integrationTestsSetup.AuthenticateAsync(RestaurantSimulationRoles.ClientRole, "test_mail@restaurant.com", Guid.NewGuid().ToString());

            var responseGet = await _integrationTestsSetup.TestClient.GetAsync("/api/auth/user");

            responseGet.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact, Order(5)]
        public async Task Should_Return_Not_Found_If_A_User_With_Specified_Id_Not_Registered()
        {
            _integrationTestsSetup.AuthenticateAsync(RestaurantSimulationRoles.AdminRole, "test_mail@restaurant.com", Guid.NewGuid().ToString());

            var responseGet = await _integrationTestsSetup.TestClient.GetAsync($"/api/auth/users/{Guid.NewGuid()}");

            responseGet.StatusCode.ShouldBe(HttpStatusCode.NotFound);

        }

        [Fact, Order(6)]
        public async Task Should_Return_User_Registered_With_Specified_Id()
        {
            var responseGetUserById = await _integrationTestsSetup.TestClient.GetAsync($"/api/auth/user/{RestaurantContextSeed.usersGuid[0]}");

            responseGetUserById.StatusCode.ShouldBe(HttpStatusCode.OK);

            var user = await responseGetUserById.Content.ReadFromJsonAsync<AuthenticationResponse>();

            user?.FirstName.ShouldBe(RestaurantContextSeed.users[0].FirstName);
            user?.PhoneNumber.ShouldBe(RestaurantContextSeed.users[0].PhoneNumber);

        }

        [Fact, Order(7)]
        public async Task Should_Update_Existing_User()
        {
            _integrationTestsSetup.AuthenticateAsync(RestaurantSimulationRoles.AdminRole, "test_mail@restaurant.com", _integrationTestsSetup.userSub);

            var responseHttpPutUser = await _integrationTestsSetup.TestClient.PutAsJsonAsync("/api/auth/user",
                CreateUpdateUserRequest("Mirel", "Danilu", "0773823902", "Piatra Neamt, Boy"));

            responseHttpPutUser.StatusCode.ShouldBe(HttpStatusCode.OK);

            var user = await responseHttpPutUser.Content.ReadFromJsonAsync<AuthenticationResponse>();

            user?.FirstName.ShouldBe("Mirel");
            user?.LastName.ShouldBe("Danilu");
            user?.Email.ShouldBe("test_mail@restaurant.com");
            user?.PhoneNumber.ShouldBe("0773823902");
            user?.Address.ShouldBe("Piatra Neamt, Boy");

            var responseGetUser = await _integrationTestsSetup.TestClient.GetAsync($"/api/auth/user");

            responseGetUser.StatusCode.ShouldBe(HttpStatusCode.OK);

            var userById = await responseGetUser.Content.ReadFromJsonAsync<AuthenticationResponse>();

            userById?.FirstName.ShouldBe("Mirel");
            userById?.LastName.ShouldBe("Danilu");
            userById?.Email.ShouldBe("test_mail@restaurant.com");
            userById?.PhoneNumber.ShouldBe("0773823902");
            userById?.Address.ShouldBe("Piatra Neamt, Boy");
        }

        [Fact, Order(8)]
        public async Task Should_Return_404_If_User_Dont_Exist_And_Want_To_Be_Updated()
        {
            _integrationTestsSetup.AuthenticateAsync(
                RestaurantSimulationRoles.ClientRole, 
                "test_mail_not_found@restaurant.com",
                Guid.NewGuid().ToString());

            var responseHttpPutUser = await _integrationTestsSetup.TestClient.PutAsJsonAsync("/api/auth/user",
                CreateRegisterUserRequest("Mirel", "Danilu", "0773823902", "Piatra Neamt, Boy"));

            responseHttpPutUser.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }


        [Fact, Order(9)]
        public async Task Should_Return_Bad_Request_If_Validations_For_First_Name_Will_Fail()
        {
            _integrationTestsSetup.AuthenticateAsync(RestaurantSimulationRoles.AdminRole, "test_mail@restaurant.com", _integrationTestsSetup.userSub);

            var responsePost = await _integrationTestsSetup.TestClient.PostAsJsonAsync("/api/auth/user",
                CreateRegisterUserRequest("", "Doctorul", "0773823901", "Piatra Neamt"));

            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            responsePost = await _integrationTestsSetup.TestClient.PostAsJsonAsync("/api/auth/user",
                CreateRegisterUserRequest("12311", "Doctorul", "0773823901", "Piatra Neamt"));

            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            responsePost = await _integrationTestsSetup.TestClient.PostAsJsonAsync("/api/auth/user",
                CreateRegisterUserRequest("Ro", "Doctorul", "0773823901", "Piatra Neamt"));

            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact, Order(10)]
        public async Task Should_Return_Bad_Request_If_Validations_For_Last_Name_Will_Fail()
        {
            _integrationTestsSetup.AuthenticateAsync(RestaurantSimulationRoles.AdminRole, "test_mail@restaurant.com", _integrationTestsSetup.userSub);

            var responsePost = await _integrationTestsSetup.TestClient.PostAsJsonAsync("/api/auth/user",
                CreateRegisterUserRequest("Mirel", "", "0773823901", "Piatra Neamt"));

            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            responsePost = await _integrationTestsSetup.TestClient.PostAsJsonAsync("/api/auth/user",
                CreateRegisterUserRequest("Mirel", "12311", "0773823901", "Piatra Neamt"));

            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            responsePost = await _integrationTestsSetup.TestClient.PostAsJsonAsync("/api/auth/user",
                CreateRegisterUserRequest("Mirel", "Do", "0773823901", "Piatra Neamt"));

            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact, Order(11)]
        public async Task Should_Return_Bad_Request_If_Validations_For_PhoneNumber_Will_Fail()
        {
            _integrationTestsSetup.AuthenticateAsync(RestaurantSimulationRoles.AdminRole, "test_mail@restaurant.com", _integrationTestsSetup.userSub);

            var responsePost = await _integrationTestsSetup.TestClient.PostAsJsonAsync("/api/auth/user",
                CreateRegisterUserRequest("Mirel", "Dorel", "07738239011", "Piatra Neamt"));

            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            responsePost = await _integrationTestsSetup.TestClient.PostAsJsonAsync("/api/auth/user",
                CreateRegisterUserRequest("Mirel", "Dorel", "0773823901a", "Piatra Neamt"));

            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            responsePost = await _integrationTestsSetup.TestClient.PostAsJsonAsync("/api/auth/user",
                CreateRegisterUserRequest("Mirel", "Dorel", "0773823^01a", "Piatra Neamt"));

            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            responsePost = await _integrationTestsSetup.TestClient.PostAsJsonAsync("/api/auth/user",
                CreateRegisterUserRequest("Mirel", "Dorel", "aaa", "Piatra Neamt"));

            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact, Order(12)]
        public async Task Should_Return_Bad_Request_If_Validations_For_Address_Will_Fail()
        {
            _integrationTestsSetup.AuthenticateAsync(RestaurantSimulationRoles.AdminRole, "test_mail@restaurant.com", _integrationTestsSetup.userSub);

            var responsePost = await _integrationTestsSetup.TestClient.PostAsJsonAsync("/api/auth/user",
                CreateRegisterUserRequest("Mirel", "Dorel", "0773823901", "heh^^"));

            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            responsePost = await _integrationTestsSetup.TestClient.PostAsJsonAsync("/api/auth/user",
                CreateRegisterUserRequest("Mirel", "Dorel", "0773823901", "Pp"));

            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }


        /**************/
        /***HELPERS****/
        /*************/
        private RegisterUserRequest CreateRegisterUserRequest(string firstName, string lastName, string phoneNumber, string address)
        {
            return new RegisterUserRequest(
                FirstName: firstName,
                LastName: lastName,
                PhoneNumber: phoneNumber,
                Address: address);
        }

        private UpdateUserRequest CreateUpdateUserRequest(string firstName, string lastName, string phoneNumber, string address)
        {
            return new UpdateUserRequest(
                FirstName: firstName,
                LastName: lastName,
                PhoneNumber: phoneNumber,
                Address: address);
        }
    }
}
