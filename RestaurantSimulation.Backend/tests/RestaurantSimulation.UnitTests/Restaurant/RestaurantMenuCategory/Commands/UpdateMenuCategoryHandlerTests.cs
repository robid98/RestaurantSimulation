using ErrorOr;
using Moq;
using RestaurantSimulation.Application.Common.Interfaces.Persistence;
using RestaurantSimulation.Application.Restaurant.RestaurantMenuCategory.Commands.UpdateMenuCategory;
using RestaurantSimulation.Application.Restaurant.RestaurantMenuCategory.Common;
using RestaurantSimulation.Domain.Entities.Restaurant;
using Shouldly;
using RestaurantSimulation.Domain.RestaurantApplicationErrors;

namespace RestaurantSimulation.UnitTests.Restaurant.RestaurantMenuCategory.Commands
{
    public class UpdateMenuCategoryHandlerTests
    {
        private Mock<IMenuCategoryRepository> _mockMenuCategoryRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;

        public UpdateMenuCategoryHandlerTests()
        {
            _mockMenuCategoryRepository = new Mock<IMenuCategoryRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
        }

        [Fact]
        public async Task Should_Update_An_Existing_Menu_Category()
        {
            // arrange
            _mockMenuCategoryRepository.Setup(x => x.GetRestaurantMenuCategoryByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new MenuCategory(Guid.NewGuid(), "Fruits", "Best Fruits"));

            _mockMenuCategoryRepository.Setup(x => x.GetRestaurantMenuCategoryByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((MenuCategory)null);

            var handler = new UpdateMenuCategoryHandler(_mockMenuCategoryRepository.Object, _mockUnitOfWork.Object);

            // act
            ErrorOr<MenuCategoryResult> result = await handler.Handle
                (new UpdateMenuCategoryCommand(Guid.NewGuid(), "New Fruits", "Fruits from Poland"), CancellationToken.None);

            // assert
            result.IsError.ShouldBeFalse();
            result.Value.Name.ShouldBe("New Fruits");
            result.Value.Description.ShouldBe("Fruits from Poland");
        }

        [Fact]
        public async Task Should_Return_Menu_Category_Not_Found()
        {
            // arrange
            _mockMenuCategoryRepository.Setup(x => x.GetRestaurantMenuCategoryByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((MenuCategory)null);

            var handler = new UpdateMenuCategoryHandler(_mockMenuCategoryRepository.Object, _mockUnitOfWork.Object);

            // act
            ErrorOr<MenuCategoryResult> result = await handler.Handle
                (new UpdateMenuCategoryCommand(Guid.NewGuid(), "New Fruits", "Fruits from Poland"), CancellationToken.None);

            // assert
            result.IsError.ShouldBeTrue();
            result.FirstError.Code.ShouldBe(Errors.RestaurantMenuCategory.NotFound.Code);
            result.FirstError.Description.ShouldBe(Errors.RestaurantMenuCategory.NotFound.Description);
        }

        [Fact]
        public async Task Should_Return_Duplicate_Menu_Category()
        {
            // arrange
            _mockMenuCategoryRepository.Setup(x => x.GetRestaurantMenuCategoryByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new MenuCategory(Guid.NewGuid(), "Fruits", "Best Fruits"));

            _mockMenuCategoryRepository.Setup(x => x.GetRestaurantMenuCategoryByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new MenuCategory(Guid.NewGuid(), "Existing Category", "Best in the world"));

            var handler = new UpdateMenuCategoryHandler(_mockMenuCategoryRepository.Object, _mockUnitOfWork.Object);

            // act
            ErrorOr<MenuCategoryResult> result = await handler.Handle
                (new UpdateMenuCategoryCommand(Guid.NewGuid(), "Existing Category", "Fruits from Poland"), CancellationToken.None);

            // assert
            result.IsError.ShouldBeTrue();
            result.FirstError.Code.ShouldBe(Errors.RestaurantMenuCategory.DuplicateRestaurantMenuCategory.Code);
            result.FirstError.Description.ShouldBe(Errors.RestaurantMenuCategory.DuplicateRestaurantMenuCategory.Description);
        }
    }
}
