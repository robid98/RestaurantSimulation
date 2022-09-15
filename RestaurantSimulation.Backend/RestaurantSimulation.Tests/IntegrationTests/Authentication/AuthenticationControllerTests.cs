﻿using RestaurantSimulation.Application.Authentication.Common;
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

            var responseGet = await TestClient.GetAsync("/api/auth/users/accesstoken");

            responseGet.StatusCode.ShouldBe(HttpStatusCode.OK);

            var user = await responseGet.Content.ReadFromJsonAsync<AuthenticationResult>();

            user?.FirstName.ShouldBe("Mirel");
            user?.PhoneNumber.ShouldBe("0773823901");
        }

        [Fact]
        public async Task Should_Return_Not_Found_When_User_Is_Not_Registered_Getting_By_Access_Token()
        {
            AuthenticateAsync(RestaurantSimulationRoles.ClientRole);

            var responseGet = await TestClient.GetAsync("/api/auth/users/accesstoken");

            responseGet.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Should_Return_Not_Found_If_A_User_With_Specified_Id_Not_Registered()
        {
            AuthenticateAsync(RestaurantSimulationRoles.AdminRole);

            var responseGet = await TestClient.GetAsync($"/api/auth/users/id/{Guid.NewGuid()}");

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

            var responseGetUserById = await TestClient.GetAsync($"/api/auth/users/id/{users?[0].Id}");

            responseGetUserById.StatusCode.ShouldBe(HttpStatusCode.OK);

            var user = await responseGetUserById.Content.ReadFromJsonAsync<AuthenticationResult>();

            user?.FirstName.ShouldBe("Mirel");
            user?.PhoneNumber.ShouldBe("0773823901");

        }

        [Fact]
        public async Task Should_Return_Bad_Request_If_Validations_For_First_Name_Will_Fail()
        {
            AuthenticateAsync(RestaurantSimulationRoles.ClientRole);

            var responsePost = await TestClient.PostAsJsonAsync("/api/auth/users/register",
                CreateRegisterRequest("", "Doctorul", "0773823901", "Piatra Neamt"));

            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            responsePost = await TestClient.PostAsJsonAsync("/api/auth/users/register",
                CreateRegisterRequest("12311", "Doctorul", "0773823901", "Piatra Neamt"));

            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            responsePost = await TestClient.PostAsJsonAsync("/api/auth/users/register",
                CreateRegisterRequest("Ro", "Doctorul", "0773823901", "Piatra Neamt"));

            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_Return_Bad_Request_If_Validations_For_Last_Name_Will_Fail()
        {
            AuthenticateAsync(RestaurantSimulationRoles.ClientRole);

            var responsePost = await TestClient.PostAsJsonAsync("/api/auth/users/register",
                CreateRegisterRequest("Mirel", "", "0773823901", "Piatra Neamt"));

            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            responsePost = await TestClient.PostAsJsonAsync("/api/auth/users/register",
                CreateRegisterRequest("Mirel", "12311", "0773823901", "Piatra Neamt"));

            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            responsePost = await TestClient.PostAsJsonAsync("/api/auth/users/register",
                CreateRegisterRequest("Mirel", "Do", "0773823901", "Piatra Neamt"));

            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_Return_Bad_Request_If_Validations_For_PhoneNumber_Will_Fail()
        {
            AuthenticateAsync(RestaurantSimulationRoles.ClientRole);

            var responsePost = await TestClient.PostAsJsonAsync("/api/auth/users/register",
                CreateRegisterRequest("Mirel", "Dorel", "07738239011", "Piatra Neamt"));

            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            responsePost = await TestClient.PostAsJsonAsync("/api/auth/users/register",
                CreateRegisterRequest("Mirel", "Dorel", "0773823901a", "Piatra Neamt"));

            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            responsePost = await TestClient.PostAsJsonAsync("/api/auth/users/register",
                CreateRegisterRequest("Mirel", "Dorel", "aaa", "Piatra Neamt"));

            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_Return_Bad_Request_If_Validations_For_Address_Will_Fail()
        {
            AuthenticateAsync(RestaurantSimulationRoles.ClientRole);

            var responsePost = await TestClient.PostAsJsonAsync("/api/auth/users/register",
                CreateRegisterRequest("Mirel", "Dorel", "0773823901", "%#$heh^^"));

            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            responsePost = await TestClient.PostAsJsonAsync("/api/auth/users/register",
                CreateRegisterRequest("Mirel", "Dorel", "0773823901", "Pp"));

            responsePost.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }


        /**************/
        /***HELPERS****/
        /*************/
        public async Task AuthenticateAndRegisterClientUser()
        {
            AuthenticateAsync(RestaurantSimulationRoles.ClientRole);

            var responsePost = await TestClient.PostAsJsonAsync("/api/auth/users/register",
                CreateRegisterRequest("Mirel", "Doctorul", "0773823901", "Piatra Neamt"));

            responsePost.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        public async Task AuthenticateAndRegisterAdminUser()
        {
            AuthenticateAsync(RestaurantSimulationRoles.AdminRole);

            var responsePost = await TestClient.PostAsJsonAsync("/api/auth/users/register",
                CreateRegisterRequest("Mirel", "Doctorul", "0773823901", "Piatra Neamt"));

            responsePost.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        private RegisterRequest CreateRegisterRequest(string firstName, string lastName, string phoneNumber, string address)
        {
            return new RegisterRequest(
                FirstName: firstName,
                LastName: lastName,
                PhoneNumber: phoneNumber,
                Address: address);
        }
    }
}