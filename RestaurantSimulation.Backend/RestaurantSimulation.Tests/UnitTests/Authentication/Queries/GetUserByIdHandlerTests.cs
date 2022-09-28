using Moq;
using RestaurantSimulation.Application.Authentication.Queries.GetUserById;
using RestaurantSimulation.Application.Common.Interfaces.Persistence;
using RestaurantSimulation.Tests.UnitTests.Mocks.Authentication;
using Shouldly;
using RestaurantSimulation.Domain.RestaurantApplicationErrors;

namespace RestaurantSimulation.Tests.UnitTests.Authentication.Queries
{
    public class GetUserByIdHandlerTests
    {
        private Mock<IUserRepository> _mockUserRepository;

        public GetUserByIdHandlerTests()
        {
            _mockUserRepository = MockUserRepository.GetUserRepository();
        }

        [Fact]
        public async Task Should_Return_With_Valid_Guid_A_Valid_User_Test_1()
        {
            // arrange
            var handler = new GetUserByIdHandler(_mockUserRepository.Object);

            // act
            var result = await handler.Handle(new GetUserByIdQuery(MockUserRepository._guid1), CancellationToken.None);

            // assert
            if(result.IsError is false)
            {
                result.Value.Email.ShouldBe("robert98@yahoo.com");
                result.Value.PhoneNumber.ShouldBe("0773111222");
            }
        }

        [Fact]
        public async Task Should_Return_With_Valid_Guid_A_Valid_User_Test_2()
        {
            // arrange 
            var handler = new GetUserByIdHandler(_mockUserRepository.Object);

            // act
            var result = await handler.Handle(new GetUserByIdQuery(MockUserRepository._guid2), CancellationToken.None);

            // assert
            if (result.IsError is false)
            {
                result.Value.Email.ShouldBe("valibujor@yahoo.com");
                result.Value.PhoneNumber.ShouldBe("0773111333");
            }
        }

        [Fact]
        public async Task Should_Return_With_Invalid_Guid_Error_Not_Found()
        {
            // arrange
            var handler = new GetUserByIdHandler(_mockUserRepository.Object);

            // act
            var result = await handler.Handle(new GetUserByIdQuery(Guid.NewGuid()), CancellationToken.None);

            // assert
            result.FirstError.Code.ShouldBe(Errors.User.NotFound.Code);
        }
    }
}
