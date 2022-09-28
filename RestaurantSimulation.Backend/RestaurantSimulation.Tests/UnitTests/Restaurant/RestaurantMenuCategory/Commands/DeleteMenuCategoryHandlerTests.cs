using ErrorOr;
using MediatR;
using Moq;
using RestaurantSimulation.Application.Common.Interfaces.Persistence;
using RestaurantSimulation.Application.Restaurant.RestaurantMenuCategory.Commands.DeleteMenuCategory;
using RestaurantSimulation.Domain.Entities.Restaurant;
using Shouldly;
using RestaurantSimulation.Domain.RestaurantApplicationErrors;

namespace RestaurantSimulation.Tests.UnitTests.Restaurant.RestaurantMenuCategory.Commands
{
    public class DeleteMenuCategoryHandlerTests
    {
        private readonly Mock<IMenuCategoryRepository> _mockMenuCategoryRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;

        public DeleteMenuCategoryHandlerTests()
        {
            _mockMenuCategoryRepository = new Mock<IMenuCategoryRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
        }

        [Fact]
        public async Task Should_Delete_An_Existing_Menu_Category()
        {
            // arrange
            _mockMenuCategoryRepository.Setup(x => x.GetRestaurantMenuCategoryByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new MenuCategory(Guid.NewGuid(), "Fruits", "Best description"));

            var handler = new DeleteMenuCategoryHandler(_mockMenuCategoryRepository.Object, _mockUnitOfWork.Object);

            // act
            ErrorOr<Unit> result = await handler.Handle(new DeleteMenuCategoryCommand(Guid.NewGuid()), CancellationToken.None);

            // assert
            result.IsError.ShouldBeFalse();
            result.Value.ShouldBe(Unit.Value);

        }

        [Fact]
        public async Task ShouldReturnMenuCategoryNotFound()
        {
            // arrange
            _mockMenuCategoryRepository.Setup(x => x.GetRestaurantMenuCategoryByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((MenuCategory?)null);

            var handler = new DeleteMenuCategoryHandler(_mockMenuCategoryRepository.Object, _mockUnitOfWork.Object);

            // act
            ErrorOr<Unit> result = await handler.Handle(new DeleteMenuCategoryCommand(Guid.NewGuid()), CancellationToken.None);

            // assert
            result.IsError.ShouldBeTrue();
            result.FirstError.Code.ShouldBe(Errors.RestaurantMenuCategory.NotFound.Code);
            result.FirstError.Description.ShouldBe(Errors.RestaurantMenuCategory.NotFound.Description);

        }
    }
}
