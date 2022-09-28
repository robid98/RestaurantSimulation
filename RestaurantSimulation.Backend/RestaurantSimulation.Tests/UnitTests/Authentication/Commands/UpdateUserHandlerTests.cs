using Moq;
using RestaurantSimulation.Application.Authentication.Commands.UpdateUser;
using RestaurantSimulation.Application.Authentication.Common.Services.ExtractUserClaims;
using RestaurantSimulation.Application.Common.Interfaces.Persistence;
using RestaurantSimulation.Domain.Entities.Authentication;
using RestaurantSimulation.Tests.UnitTests.Mocks.Authentication;
using Shouldly;
using RestaurantSimulation.Domain.RestaurantApplicationErrors;

namespace RestaurantSimulation.Tests.UnitTests.Authentication.Commands
{
    public class UpdateUserHandlerTests
    {
        private Mock<IUserRepository> _mockUserRepository;
        private Mock<IExtractUserClaimsService> _extractUserClaimsService;
        private Mock<IUnitOfWork> _unitOfWork;

        public UpdateUserHandlerTests()
        {
            _mockUserRepository = MockUserRepository.GetUserRepository();
            _extractUserClaimsService = new Mock<IExtractUserClaimsService>();
            _unitOfWork = new Mock<IUnitOfWork>();
        }

        [Fact]
        public async Task Should_Update_Current_User_Profile()
        {
            // arrange
            _extractUserClaimsService.Setup(x => x.GetUserEmail()).Returns("robert98@yahoo.com");
            _extractUserClaimsService.Setup(x => x.GetUserSub()).Returns("restaurant|usertest");

            var handler = new UpdateUserHandler(
                _mockUserRepository.Object, 
                _extractUserClaimsService.Object,
                _unitOfWork.Object);

            // act
            var result = await handler.Handle(new UpdateUserCommand(
                    FirstName: "Costelus",
                    LastName: "Barbosul",
                    PhoneNumber: "1111111111",
                    Address: "Schimbata Adresa"), CancellationToken.None);

            // assert
            if (result.IsError is false)
            {
                result.Value.FirstName.ShouldBe("Costelus");
                result.Value.LastName.ShouldBe("Barbosul");
                result.Value.PhoneNumber.ShouldBe("1111111111");
                result.Value.Address.ShouldBe("Schimbata Adresa");
            }
        }

        [Fact]
        public async Task Should_Return_404_If_User_Dont_Exist_And_Want_To_Be_Updated()
        {
            // arrange
            _extractUserClaimsService.Setup(x => x.GetUserEmail()).Returns("robert98@yahoo.com");
            _extractUserClaimsService.Setup(x => x.GetUserSub()).Returns("restaurant|usertest");

            _mockUserRepository.Setup(x => x.GetUserByEmailAsync(It.IsAny<string>())).Returns(Task.FromResult<User?>(null));

            var handler = new UpdateUserHandler(
                _mockUserRepository.Object, 
                _extractUserClaimsService.Object,
                _unitOfWork.Object);

            // act
            var result = await handler.Handle(new UpdateUserCommand(
                    FirstName: "Costelus",
                    LastName: "Barbosul",
                    PhoneNumber: "1111111111",
                    Address: "Schimbata Adresa"), CancellationToken.None);

            // assert
            result.IsError.ShouldBe(true);
            result.FirstError.Code.ShouldBe(Errors.User.NotFound.Code);
        }
    }
}
