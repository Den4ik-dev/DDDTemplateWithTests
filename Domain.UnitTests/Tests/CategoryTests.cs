using Domain.Product.ValueObject;
using FluentAssertions;

namespace Domain.UnitTests.Tests;

public class CategoryTests
{
    [Fact]
    public void Add_a_product_id_to_a_category()
    {
        // Arrange
        ProductId productId = ProductId.Create(Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"));
        Category.Category sut = Category.Category.Create("Phones");

        // Act
        sut.AddProductId(productId);

        // Assert
        sut.ProductIds.Count().Should().Be(1);
        sut.ProductIds.Count(pi => pi.Value == Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"))
            .Should()
            .Be(1);
    }
}
