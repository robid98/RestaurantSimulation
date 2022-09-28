﻿using ErrorOr;
using Moq;
using RestaurantSimulation.Application.Common.Interfaces.Persistence;
using RestaurantSimulation.Application.Restaurant.RestaurantMenuCategory.Queries.GetProductsMenuCategoryById;
using RestaurantSimulation.Application.Restaurant.RestaurantProducts.Common;
using RestaurantSimulation.Domain.Entities.Restaurant;
using Shouldly;
using RestaurantSimulation.Domain.RestaurantApplicationErrors;

namespace RestaurantSimulation.Tests.UnitTests.Restaurant.RestaurantMenuCategory.Queries
{
    public class GetProductsMenuCategoryByIdHandlerTests
    {
        private Mock<IMenuCategoryRepository> _mockMenuCategoryRepository;

        public GetProductsMenuCategoryByIdHandlerTests()
        {
            _mockMenuCategoryRepository = new Mock<IMenuCategoryRepository>();
        }

        [Fact]
        public async Task Should_Return_A_List_Of_Products()
        {
            // arrange
            _mockMenuCategoryRepository.Setup(r => r.GetProductsRestaurantMenuCategoryByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new List<Product> 
                {
                        new Product(
                            Guid.NewGuid(),
                            15.35,
                            "Omleta",
                            true,
                            Guid.NewGuid()
                       )
                });

            var handler = new GetProductsMenuCategoryByIdHandler(_mockMenuCategoryRepository.Object);

            // act 
            var result = await handler.Handle(new GetProductsMenuCategoryByIdQuery(Guid.NewGuid()), CancellationToken.None);

            // assert
            result.ShouldBeOfType<ErrorOr<List<ProductResult>>>();
            result.IsError.ShouldBeFalse();

            result.Value[0].Price.ShouldBe(15.35);
            result.Value[0].Description.ShouldBe("Omleta");
        }

        [Fact]
        public async Task Should_Return_A_Empty_Products_List()
        {
            // arrange
            _mockMenuCategoryRepository.Setup(r => r.GetProductsRestaurantMenuCategoryByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((List<Product>?)new List<Product> { });


            var handler = new GetProductsMenuCategoryByIdHandler(_mockMenuCategoryRepository.Object);

            // act 
            var result = await handler.Handle(new GetProductsMenuCategoryByIdQuery(Guid.NewGuid()), CancellationToken.None);

            // assert
            result.ShouldBeOfType<ErrorOr<List<ProductResult>>>();
            result.IsError.ShouldBeFalse();
            result.Value.Count.ShouldBe(0);
        }

        [Fact]
        public async Task Should_Return_Menu_Category_Not_Found()
        {
            // arrange
            _mockMenuCategoryRepository.Setup(r => r.GetProductsRestaurantMenuCategoryByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((List<Product>?)null);
                

            var handler = new GetProductsMenuCategoryByIdHandler(_mockMenuCategoryRepository.Object);

            // act 
            var result = await handler.Handle(new GetProductsMenuCategoryByIdQuery(Guid.NewGuid()), CancellationToken.None);

            // assert
            result.ShouldBeOfType<ErrorOr<List<ProductResult>>>();
            result.IsError.ShouldBeTrue();
            result.FirstError.Code.ShouldBe(Errors.RestaurantMenuCategory.NotFound.Code);
            result.FirstError.Description.ShouldBe(Errors.RestaurantMenuCategory.NotFound.Description);
        }
    }
}
