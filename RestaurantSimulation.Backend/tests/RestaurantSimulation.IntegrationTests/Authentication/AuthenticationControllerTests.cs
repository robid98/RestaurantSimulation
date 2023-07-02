﻿using System.Net;
using System.Net.Http.Json;
using Shouldly;
using RestaurantSimulation.Contracts.Authentication;
using RestaurantSimulation.Domain.Common.Roles;
using RestaurantSimulation.IntegrationTests.Helpers;

namespace RestaurantSimulation.IntegrationTests.Authentication
{
    public class AuthenticationControllerTests : CustomWebApplicationBase, IAsyncLifetime
    {
        private RegisterUserRequest userRequest = new RegisterUserRequest(
                "Mirel", "Doctorul", "0773823901", "Piatra Neamt"
        );
            
        public AuthenticationControllerTests(CustomWebApplicationFactory<Program> factory) : base(factory)
        {

        }

        [Fact]
        public async Task PostAsJsonAsync_WithValidUserInformations_ShouldCreateANewUser()
        {
            AuthenticateAsync(RestaurantSimulationRoles.AdminRole, "test_mail@restaurant.com", _userSub);

            var responsePost = await _httpClient.PostAsJsonAsync("/api/auth/user",
                CreateRegisterUserRequest(userRequest.FirstName, userRequest.LastName, userRequest.PhoneNumber, userRequest.Address));

            responsePost.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetAsync_WhenUserHaveAdminRole_ShouldReturnAllTheUsers()
        {
            AuthenticateAsync(RestaurantSimulationRoles.AdminRole, "test_mail@restaurant.com", _userSub);

            var responsePost = await _httpClient.PostAsJsonAsync("/api/auth/user",
                CreateRegisterUserRequest(userRequest.FirstName, userRequest.LastName, userRequest.PhoneNumber, userRequest.Address));

            responsePost.StatusCode.ShouldBe(HttpStatusCode.OK);

            var responseGet = await _httpClient.GetAsync("/api/auth/users/");

            responseGet.StatusCode.ShouldBe(HttpStatusCode.OK);

            var users = await responseGet.Content.ReadFromJsonAsync<List<AuthenticationResponse>>();

            users.Count.ShouldBe(1);
            users[0].FirstName.ShouldBe(userRequest.FirstName);
        }

        [Fact]
        public async Task GetAsync_WhenUserAccessTokenIsValidAndUserIsRegistered_ShouldReturnUserInformationsBasedOnSub()
        {
            AuthenticateAsync(RestaurantSimulationRoles.AdminRole, "test_mail@restaurant.com", _userSub);

            var responsePost = await _httpClient.PostAsJsonAsync("/api/auth/user",
                CreateRegisterUserRequest(userRequest.FirstName, userRequest.LastName, userRequest.PhoneNumber, userRequest.Address));

            responsePost.StatusCode.ShouldBe(HttpStatusCode.OK);

            var responseGet = await _httpClient.GetAsync("/api/auth/user");

            responseGet.StatusCode.ShouldBe(HttpStatusCode.OK);

            var user = await responseGet.Content.ReadFromJsonAsync<AuthenticationResponse>();

            user?.FirstName.ShouldBe(userRequest.FirstName);
            user?.PhoneNumber.ShouldBe(userRequest.PhoneNumber);
        }

        [Fact]
        public async Task GetAsync_WhenUserAccessTokenIsValidButIsNotRegistered_ShouldReturnNotFound()
        {
            AuthenticateAsync(RestaurantSimulationRoles.ClientRole, "test_mail@restaurant.com", Guid.NewGuid().ToString());

            var responseGet = await _httpClient.GetAsync("/api/auth/user");

            responseGet.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetAsync_WhenCalledWithAInvalidUserId_ShouldReturnNotFound()
        {
            AuthenticateAsync(RestaurantSimulationRoles.AdminRole, "test_mail@restaurant.com", Guid.NewGuid().ToString());

            var responseGet = await _httpClient.GetAsync($"/api/auth/users/{Guid.NewGuid()}");

            responseGet.StatusCode.ShouldBe(HttpStatusCode.NotFound);

        }

        [Fact]
        public async Task GetAsync_WhenCalledWithAValidUserIdAndUserHaveAdminRole_ShouldReturnUserInformations()
        {
            AuthenticateAsync(RestaurantSimulationRoles.AdminRole, "test_mail@restaurant.com", Guid.NewGuid().ToString());

            var responsePost = await _httpClient.PostAsJsonAsync("/api/auth/user",
                CreateRegisterUserRequest(userRequest.FirstName, userRequest.LastName, userRequest.PhoneNumber, userRequest.Address));

            responsePost.StatusCode.ShouldBe(HttpStatusCode.OK);

            var responseGetUser = await _httpClient.GetAsync($"/api/auth/user");

            responseGetUser.StatusCode.ShouldBe(HttpStatusCode.OK);

            var userById = await responseGetUser.Content.ReadFromJsonAsync<AuthenticationResponse>();

            var responseGetUserById = await _httpClient.GetAsync($"/api/auth/user/{userById.Id}");

            responseGetUserById.StatusCode.ShouldBe(HttpStatusCode.OK);

            var user = await responseGetUserById.Content.ReadFromJsonAsync<AuthenticationResponse>();

            user.FirstName.ShouldBe("Mirel");
        }

        [Fact]
        public async Task PutAsJsonAsync_WhenCalledWithValidInformations_ShouldUpdateCurrentUserBasedOnSub()
        {
            AuthenticateAsync(RestaurantSimulationRoles.AdminRole, "test_mail@restaurant.com", _userSub);

            var responsePost = await _httpClient.PostAsJsonAsync("/api/auth/user",
                CreateRegisterUserRequest(userRequest.FirstName, userRequest.LastName, userRequest.PhoneNumber, userRequest.Address));

            responsePost.StatusCode.ShouldBe(HttpStatusCode.OK);

            var responseHttpPutUser = await _httpClient.PutAsJsonAsync("/api/auth/user",
                CreateUpdateUserRequest("Mirel", "Danilu", "0773823902", "Piatra Neamt, Boy"));

            responseHttpPutUser.StatusCode.ShouldBe(HttpStatusCode.OK);

            var responseGetUser = await _httpClient.GetAsync($"/api/auth/user");

            responseGetUser.StatusCode.ShouldBe(HttpStatusCode.OK);

            var userById = await responseGetUser.Content.ReadFromJsonAsync<AuthenticationResponse>();

            userById?.FirstName.ShouldBe("Mirel");
            userById?.LastName.ShouldBe("Danilu");
            userById?.Email.ShouldBe("test_mail@restaurant.com");
            userById?.PhoneNumber.ShouldBe("0773823902");
            userById?.Address.ShouldBe("Piatra Neamt, Boy");
        }

        [Fact]
        public async Task PutAsJsonAsync_WhenUserDoesntExist_ShouldReturnNotFound()
        {
            AuthenticateAsync(
                RestaurantSimulationRoles.ClientRole, 
                "test_mail_not_found@restaurant.com",
                Guid.NewGuid().ToString());

            var responseHttpPutUser = await _httpClient.PutAsJsonAsync("/api/auth/user",
                CreateRegisterUserRequest("Mirel", "Danilu", "0773823902", "Piatra Neamt, Boy"));

            responseHttpPutUser.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }


        [Fact]
        public async Task PostAsJsonAsync_WhenFirstNameIsInvalid_ShouldReturnBadRequest()
        {
            AuthenticateAsync(RestaurantSimulationRoles.AdminRole, "test_mail@restaurant.com", _userSub);

            var responsePost = await _httpClient.PostAsJsonAsync("/api/auth/user",
                CreateRegisterUserRequest("", "Doctorul", "0773823901", "Piatra Neamt"));

            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            responsePost = await _httpClient.PostAsJsonAsync("/api/auth/user",
                CreateRegisterUserRequest("12311", "Doctorul", "0773823901", "Piatra Neamt"));

            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            responsePost = await _httpClient.PostAsJsonAsync("/api/auth/user",
                CreateRegisterUserRequest("Ro", "Doctorul", "0773823901", "Piatra Neamt"));

            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task PostAsJsonAsync_WhenLastNameIsInvalid_ShouldReturnBadRequest()
        {
            AuthenticateAsync(RestaurantSimulationRoles.AdminRole, "test_mail@restaurant.com", _userSub);

            var responsePost = await _httpClient.PostAsJsonAsync("/api/auth/user",
                CreateRegisterUserRequest("Mirel", "", "0773823901", "Piatra Neamt"));

            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            responsePost = await _httpClient.PostAsJsonAsync("/api/auth/user",
                CreateRegisterUserRequest("Mirel", "12311", "0773823901", "Piatra Neamt"));

            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            responsePost = await _httpClient.PostAsJsonAsync("/api/auth/user",
                CreateRegisterUserRequest("Mirel", "Do", "0773823901", "Piatra Neamt"));

            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task PostAsJsonAsync_WhenPhoneNumberIsInvalid_ShouldReturnBadRequest()
        {
            AuthenticateAsync(RestaurantSimulationRoles.AdminRole, "test_mail@restaurant.com", _userSub);

            var responsePost = await _httpClient.PostAsJsonAsync("/api/auth/user",
                CreateRegisterUserRequest("Mirel", "Dorel", "07738239011", "Piatra Neamt"));

            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            responsePost = await _httpClient.PostAsJsonAsync("/api/auth/user",
                CreateRegisterUserRequest("Mirel", "Dorel", "0773823901a", "Piatra Neamt"));

            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            responsePost = await _httpClient.PostAsJsonAsync("/api/auth/user",
                CreateRegisterUserRequest("Mirel", "Dorel", "0773823^01a", "Piatra Neamt"));

            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            responsePost = await _httpClient.PostAsJsonAsync("/api/auth/user",
                CreateRegisterUserRequest("Mirel", "Dorel", "aaa", "Piatra Neamt"));

            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task PostAsJsonAsync_WhenAddressIsInvalid_ShouldReturnBadRequest()
        {
            AuthenticateAsync(RestaurantSimulationRoles.AdminRole, "test_mail@restaurant.com", _userSub);

            var responsePost = await _httpClient.PostAsJsonAsync("/api/auth/user",
                CreateRegisterUserRequest("Mirel", "Dorel", "0773823901", "heh^^"));

            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            responsePost = await _httpClient.PostAsJsonAsync("/api/auth/user",
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
