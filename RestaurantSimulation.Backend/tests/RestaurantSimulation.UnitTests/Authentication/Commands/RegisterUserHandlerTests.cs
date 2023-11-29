using Moq;
using RestaurantSimulation.Application.Authentication.Commands.RegisterUser;
using RestaurantSimulation.Application.Common.Interfaces.Persistence;
using RestaurantSimulation.Domain.Entities.Authentication;
using RestaurantSimulation.UnitTests.Mocks.Authentication;
using Shouldly;
using RestaurantSimulation.Domain.RestaurantApplicationErrors;
using RestaurantSimulation.Application.Authentication.Common.Services.ExtractUserClaims;
using Microsoft.Extensions.Logging;

namespace RestaurantSimulation.UnitTests.Authentication.Commands
{
    public class RegisterUserHandlerTests
    {
        private Mock<IUserRepository> _mockUserRepository;
        private Mock<IExtractUserClaimsService> _extractUserClaimsService;
        private Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<ILogger<RegisterUserHandler>> _logger;

        public RegisterUserHandlerTests()
        {
            _mockUserRepository = MockUserRepository.GetUserRepository();
            _extractUserClaimsService = new Mock<IExtractUserClaimsService>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _logger = new Mock<ILogger<RegisterUserHandler>>();
        }

        [Fact]
        public async Task RegisterUserHandler_WhenDetailsAreValid_ShouldAddANewUser()
        {
            // arrange
            _extractUserClaimsService.Setup(x => x.GetUserEmail()).Returns("test@email.com");
            _extractUserClaimsService.Setup(x => x.GetUserSub()).Returns("restaurant|usertest");

            var handler = new RegisterUserHandler(
                _mockUserRepository.Object, 
                _extractUserClaimsService.Object, 
                _unitOfWork.Object,
                _logger.Object);

            // act
            var result = await handler.Handle(new RegisterUserCommand(
                    FirstName: "Gigel",
                    LastName: "Fronea",
                    PhoneNumber: "Auth0:TestNumber",
                    Address: "Beatiful Place"), CancellationToken.None);

            // assert
            if (result.IsError is false)
            {
                MockUserRepository.userList.Count.ShouldBe(3);
                MockUserRepository.userList[MockUserRepository.userList.Count - 1].FirstName.ShouldBe("Gigel");
            }
        }

        [Fact]
        public async Task RegisterUserHandler_WhenUserAlreadyExists_ShouldReturnErrorDuplicateEmail()
        {
            // arrange
            _mockUserRepository = new Mock<IUserRepository>();

            _mockUserRepository.Setup(r => r.GetUserByEmailAsync(It.IsAny<string>())).ReturnsAsync(new User(
                Guid.NewGuid(),
                "auth0|test",
                "testemail@restaurant.com",
                "Gigel",
                "Fronea",
                "Auth0:TestNumber",
                "Beatiful Place"
            ));

            var handler = new RegisterUserHandler(
                _mockUserRepository.Object, 
                _extractUserClaimsService.Object,
                _unitOfWork.Object,
                _logger.Object);

            // act
            var result = await handler.Handle(new RegisterUserCommand(
                    FirstName: "Gigel",
                    LastName: "Fronea",
                    PhoneNumber: "Auth0:TestNumber",
                    Address: "Beatiful Place"), CancellationToken.None);

            // assert
            result.IsError.ShouldBe(true);
            result.FirstError.Code.ShouldBe(Errors.User.DuplicateEmail.Code);
        }

    }
}
