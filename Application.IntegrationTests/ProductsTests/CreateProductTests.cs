using Application.IntegrationTests.Common;
using Application.Products.Commands.CreateProduct;
using Domain.Category;
using Domain.Product;
using Domain.Product.ValueObject;
using FluentAssertions;
using FluentResults;
using Infrastructure.Data;
using MediatR;

namespace Application.IntegrationTests.ProductsTests;

public class CreateProductTests : BaseIntegrationTest
{
    public CreateProductTests(IntegrationTestWebApplicationFactory factory)
        : base(factory) { }

    [Fact]
    public async Task Get_a_product_id_when_creating_a_product_with_valid_data()
    {
        // Arrange
        Category category = Category.Create("Phones");

        using (ApplicationDbContext context = CreateDbContext())
        {
            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();
        }

        var command = new CreateProductCommand(
            "IPhone 13 pro",
            "512/12 gb",
            100_000,
            category.Id.Value
        );

        // Act
        Result<ProductId> result = await Sender.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();

        using (ApplicationDbContext context = CreateDbContext())
        {
            ProductId productId = result.Value;

            context.Products.Count().Should().Be(1);

            Product product = (await context.Products.FindAsync(productId))!;

            product.Name.Should().Be("IPhone 13 pro");
            product.Description.Should().Be("512/12 gb");
            product.Price.Value.Should().Be(100_000);
            product.CategoryId.Should().Be(category.Id);

            Category updatedCategory = (await context.Categories.FindAsync(category.Id))!;
            updatedCategory.ProductIds.Count.Should().Be(1);
            updatedCategory.ProductIds.Count(pi => pi == productId).Should().Be(1);
        }
    }

    [Fact]
    public async Task Get_an_error_when_creating_a_product_with_invalid_category_id()
    {
        // Arrange
        var command = new CreateProductCommand(
            "IPhone 13 pro",
            "512/18 gb",
            100_000,
            Guid.NewGuid()
        );

        // Act
        Result<ProductId> result = await Sender.Send(command);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Single().Message.Should().Be("Category with id not found");

        using (ApplicationDbContext context = CreateDbContext())
        {
            context.Products.Count().Should().Be(0);
        }
    }

    [Fact]
    public async Task Get_an_error_when_creating_a_product_with_a_name_that_already_exists()
    {
        // Arrange
        var storedCategory = Category.Create("Phones");

        var storedProduct = Product.Create(
            "IPhone 13 pro",
            "512/18 gb",
            Price.Create(100_000),
            storedCategory.Id
        );

        using (ApplicationDbContext context = CreateDbContext())
        {
            await context.Categories.AddAsync(storedCategory);
            await context.Products.AddAsync(storedProduct);
            await context.SaveChangesAsync();

            storedCategory.AddProductId(storedProduct.Id);
            await context.SaveChangesAsync();
        }

        var command = new CreateProductCommand(
            "IPhone 13 pro",
            "512/18 gb",
            100_000,
            storedCategory.Id.Value
        );

        // Act
        Result<ProductId> result = await Sender.Send(command);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Single().Message.Should().Be("Product with name already exists");

        using (ApplicationDbContext context = CreateDbContext())
        {
            context.Products.Count().Should().Be(1);
            context.Products.Count(p => p.Name == "IPhone 13 pro").Should().Be(1);

            Category storedCategoryFromDatabase = (
                await context.Categories.FindAsync(storedCategory.Id)
            )!;

            storedCategoryFromDatabase.ProductIds.Count.Should().Be(1);
            storedCategoryFromDatabase
                .ProductIds.Count(pi => pi == storedProduct.Id)
                .Should()
                .Be(1);
        }
    }
}
