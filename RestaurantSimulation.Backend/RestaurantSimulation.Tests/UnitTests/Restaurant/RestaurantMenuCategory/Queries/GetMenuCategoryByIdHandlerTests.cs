using ErrorOr;
using Moq;
using RestaurantSimulation.Application.Common.Interfaces.Persistence;
using RestaurantSimulation.Application.Restaurant.RestaurantMenuCategory.Common;
using RestaurantSimulation.Application.Restaurant.RestaurantMenuCategory.Queries.GetMenuCategoryById;
using RestaurantSimulation.Domain.Entities.Restaurant;
using Shouldly;
using RestaurantSimulation.Domain.RestaurantApplicationErrors;

namespace RestaurantSimulation.Tests.UnitTests.Restaurant.RestaurantMenuCategory.Queries
{
    public class GetMenuCategoryByIdHandlerTests
    {
        private Mock<IMenuCategoryRepository> _mockMenuCategoryRepository;

        public GetMenuCategoryByIdHandlerTests()
        {
            _mockMenuCategoryRepository = new Mock<IMenuCategoryRepository>();
        }

        [Fact]
        public async Task Should_Return_A_Valid_Menu_Category()
        {
            // arrange
            _mockMenuCategoryRepository.Setup(r => r.GetRestaurantMenuCategoryByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new MenuCategory(
                    Guid.NewGuid(),
                    "Mancari de apa",
                    "Cele mai bune fructe de apa"
                 ));

            var handler = new GetMenuCategoryByIdHandler(_mockMenuCategoryRepository.Object);

            // act 
            var result = await handler.Handle(new GetMenuCategoryByIdQuery(Guid.NewGuid()), CancellationToken.None);

            // assert
            result.ShouldBeOfType<ErrorOr<MenuCategoryResult>>();
            result.IsError.ShouldBeFalse();

            result.Value.Name.ShouldBe("Mancari de apa");
            result.Value.Description.ShouldBe("Cele mai bune fructe de apa");
        }

        [Fact]
        public async Task Should_Return_Menu_Category_Not_Found()
        {
            // arrange
            _mockMenuCategoryRepository.Setup(r => r.GetRestaurantMenuCategoryByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((MenuCategory?)null);

            var handler = new GetMenuCategoryByIdHandler(_mockMenuCategoryRepository.Object);

            // act 
            var result = await handler.Handle(new GetMenuCategoryByIdQuery(Guid.NewGuid()), CancellationToken.None);

            // assert
            result.ShouldBeOfType<ErrorOr<MenuCategoryResult>>();
            result.IsError.ShouldBeTrue();
            result.FirstError.Code.ShouldBe(Errors.RestaurantMenuCategory.NotFound.Code);
            result.FirstError.Description.ShouldBe(Errors.RestaurantMenuCategory.NotFound.Description);
        }
    }


}
