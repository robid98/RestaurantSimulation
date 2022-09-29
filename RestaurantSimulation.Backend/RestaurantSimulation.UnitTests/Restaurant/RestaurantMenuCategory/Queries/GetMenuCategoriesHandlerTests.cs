using ErrorOr;
using Moq;
using RestaurantSimulation.Application.Common.Interfaces.Persistence;
using RestaurantSimulation.Application.Restaurant.RestaurantMenuCategory.Common;
using RestaurantSimulation.Application.Restaurant.RestaurantMenuCategory.Queries.GetMenuCategories;
using RestaurantSimulation.Domain.Entities.Restaurant;
using Shouldly;

namespace RestaurantSimulation.UnitTests.Restaurant.RestaurantMenuCategory.Queries
{
    public class GetMenuCategoriesHandlerTests
    {
        private Mock<IMenuCategoryRepository> _mockMenuCategoryRepository;

        public GetMenuCategoriesHandlerTests()
        {
            _mockMenuCategoryRepository = new Mock<IMenuCategoryRepository>();
        }

        [Fact]
        public async Task Should_Return_A_List_Of_Menu_Categories()
        {
            // arrange
            List<MenuCategory> categories = new List<MenuCategory> {
                new MenuCategory (
                    Guid.NewGuid(),
                    "Mancari de apa",
                    "Cele mai bune fructe de apa"
                 )
            };

            _mockMenuCategoryRepository.Setup(r => r.GetRestaurantMenuCategoriesAsync()).ReturnsAsync(categories);

            var handler = new GetMenuCategoriesHandler(_mockMenuCategoryRepository.Object);

            // act
            var result = await handler.Handle(new GetMenuCategoriesQuery(), CancellationToken.None);

            // assert
            result.ShouldBeOfType<ErrorOr<List<MenuCategoryResult>>>();
            result.IsError.ShouldBeFalse();
            result.Value.Count.ShouldBe(1);
            result.Value[0].Name.ShouldBe("Mancari de apa");
            result.Value[0].Description.ShouldBe("Cele mai bune fructe de apa");
        }

        [Fact]
        public async Task Should_Return_A_Empty_List()
        {
            // arrange
            _mockMenuCategoryRepository.Setup(r => r.GetRestaurantMenuCategoriesAsync()).ReturnsAsync(new List<MenuCategory> { });

            var handler = new GetMenuCategoriesHandler(_mockMenuCategoryRepository.Object);

            // act
            var result = await handler.Handle(new GetMenuCategoriesQuery(), CancellationToken.None);

            // assert
            result.ShouldBeOfType<ErrorOr<List<MenuCategoryResult>>>();
            result.IsError.ShouldBeFalse();
            result.Value.Count.ShouldBe(0);
        }
    }
}
