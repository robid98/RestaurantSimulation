﻿using RestaurantSimulation.Application.Common.Interfaces.Persistence;
using Moq;
using RestaurantSimulation.Tests.UnitTests.Mocks.Authentication;
using RestaurantSimulation.Application.Authentication.Queries.GetUsers;
using Shouldly;
using RestaurantSimulation.Application.Authentication.Common;
using ErrorOr;
using RestaurantSimulation.Domain.Entities.Authentication;

namespace RestaurantSimulation.Tests.UnitTests.Authentication.Queries
{
    public class GetUsersHandlerTests
    {
        private Mock<IUserRepository>? _mockUserRepository;

        public GetUsersHandlerTests()
        {

        }

        [Fact]
        public async Task Should_Return_List_Of_Users()
        {
            List<User> users = new List<User> {
                new User (
                    Guid.NewGuid(),
                    "auth0|testsub",
                    "robert98@yahoo.com",
                    "Robert",
                    "Darabana",
                    "Piatra Neamt",
                    "0773111222"
                 )
            };

            _mockUserRepository = MockUserRepository.GetUserRepository(users);

            var handler = new GetUsersHandler(_mockUserRepository.Object);

            var result = await handler.Handle(new GetUsersQuery(), CancellationToken.None);

            result.ShouldBeOfType<ErrorOr<List<AuthenticationResult>>>();

            if (result.IsError is false)
            {
                result.Value.Count.ShouldBe(1);
                result.Value[0].Email.ShouldBe("robert98@yahoo.com");
            }
        }

        [Fact]
        public async Task Should_Return_Empty_List_Of_Users()
        {
            _mockUserRepository = MockUserRepository.GetUserRepository(new());

            var handler = new GetUsersHandler(_mockUserRepository.Object);

            var result = await handler.Handle(new GetUsersQuery(), CancellationToken.None);

            result.ShouldBeOfType<ErrorOr<List<AuthenticationResult>>>();

            if (result.IsError is false)
            {
                result.Value.Count.ShouldBe(0);
            }
        }
    }
}
