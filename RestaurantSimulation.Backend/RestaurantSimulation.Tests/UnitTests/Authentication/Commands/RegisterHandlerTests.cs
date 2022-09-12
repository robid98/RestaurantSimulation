using Moq;
using RestaurantSimulation.Application.Authentication.Commands.Register;
using RestaurantSimulation.Application.Common.Interfaces.Persistence;
using RestaurantSimulation.Domain.Entities.Authentication;
using RestaurantSimulation.Tests.UnitTests.Mocks.Authentication;
using Shouldly;
using RestaurantSimulation.Domain.Common.Errors;

namespace RestaurantSimulation.Tests.UnitTests.Authentication.Commands
{
    public class RegisterHandlerTests
    {
        private Mock<IUserRepository> _mockUserRepository;

        public RegisterHandlerTests()
        {
            _mockUserRepository = MockUserRepository.GetUserRepository();
        }

        [Fact]
        public async Task Should_Add_A_New_User_To_User_List()
        {
            var handler = new RegisterHandler(_mockUserRepository.Object);

            var result = await handler.Handle(new RegisterCommand(
                    Sub: "Auth0:Test",
                    Email: "Auth0:TestEmail",
                    FirstName: "Gigel",
                    LastName: "Fronea",
                    PhoneNumber: "Auth0:TestNumber",
                    Address: "Beatiful Place"), CancellationToken.None);

            if (result.IsError is false)
            {
                MockUserRepository.userList.Count.ShouldBe(3);
                MockUserRepository.userList[MockUserRepository.userList.Count - 1].FirstName.ShouldBe("Gigel");
            }
        }

        [Fact]
        public async Task Should_Return_Error_Duplicate_Email_If_The_User_Already_Exist()
        {
            _mockUserRepository = new Mock<IUserRepository>();

            _mockUserRepository.Setup(r => r.GetUserByEmailAsync(It.IsAny<string>())).ReturnsAsync(new User());

            var handler = new RegisterHandler(_mockUserRepository.Object);

            var result = await handler.Handle(new RegisterCommand(
                    Sub: "Auth0:Test",
                    Email: "Auth0:TestEmail",
                    FirstName: "Gigel",
                    LastName: "Fronea",
                    PhoneNumber: "Auth0:TestNumber",
                    Address: "Beatiful Place"), CancellationToken.None);

            result.IsError.ShouldBe(true);
            result.FirstError.Code.ShouldBe(Errors.User.DuplicateEmail.Code);
        }

    }
}
