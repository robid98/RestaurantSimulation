using ErrorOr;
using Moq;
using RestaurantSimulation.Application.Common.Interfaces.Persistence;
using RestaurantSimulation.Application.Restaurant.RestaurantMenuCategory.Commands.CreateMenuCategory;
using RestaurantSimulation.Application.Restaurant.RestaurantMenuCategory.Common;
using RestaurantSimulation.Domain.Entities.Restaurant;
using Shouldly;
using RestaurantSimulation.Domain.RestaurantApplicationErrors;

namespace RestaurantSimulation.Tests.UnitTests.Restaurant.RestaurantMenuCategory.Commands
{
    public class CreateMenuCategoryHandlerTests
    {
        private Mock<IMenuCategoryRepository> _mockMenuCategoryRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;

        public CreateMenuCategoryHandlerTests()
        {
            _mockMenuCategoryRepository = new Mock<IMenuCategoryRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
        }

        [Fact]
        public async Task Should_Create_A_New_Menu_Category()
        {
            // arrange
            _mockMenuCategoryRepository.Setup(x => x.AddAsync(It.IsAny<MenuCategory>()));
            _mockMenuCategoryRepository.Setup(x => x.GetRestaurantMenuCategoryByNameAsync(It.IsAny<string>())).ReturnsAsync((MenuCategory?)null);

            var handler = new CreateMenuCategoryHandler(_mockMenuCategoryRepository.Object, _mockUnitOfWork.Object);

            // act
            ErrorOr<MenuCategoryResult> result = await handler.Handle
                (new CreateMenuCategoryCommand("Fruits", "Best Fruits"), CancellationToken.None);

            // assert
            result.IsError.ShouldBeFalse();
            result.Value.Name.ShouldBe("Fruits");
            result.Value.Description.ShouldBe("Best Fruits");
        }

        [Fact]
        public async Task Should_Return_Duplicate_Category_Name()
        {
            // arrange
            _mockMenuCategoryRepository.Setup(x => x.AddAsync(It.IsAny<MenuCategory>()));
            _mockMenuCategoryRepository.Setup(x => x.GetRestaurantMenuCategoryByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((MenuCategory?)new MenuCategory(Guid.NewGuid(), "Fruits", "Best Fruits"));

            var handler = new CreateMenuCategoryHandler(_mockMenuCategoryRepository.Object, _mockUnitOfWork.Object);

            // act
            ErrorOr<MenuCategoryResult> result = await handler.Handle
                (new CreateMenuCategoryCommand("Fruits", "Best Fruits"), CancellationToken.None);

            // assert
            result.IsError.ShouldBeTrue();
            result.FirstError.Code.ShouldBe(Errors.RestaurantMenuCategory.DuplicateRestaurantMenuCategory.Code);
            result.FirstError.Description.ShouldBe(Errors.RestaurantMenuCategory.DuplicateRestaurantMenuCategory.Description);
        }
    }
}

