using Application.IntegrationTests.Common;
using Application.Products.Commands.ChangeProduct;
using Domain.Category;
using Domain.Product;
using Domain.Product.ValueObject;
using FluentAssertions;
using FluentResults;
using Infrastructure.Data;
using MediatR;

namespace Application.IntegrationTests.ProductsTests;

public class ChangeProductTests : BaseIntegrationTest
{
    public ChangeProductTests(IntegrationTestWebApplicationFactory factory)
        : base(factory) { }

    [Fact]
    public async Task Successful_full_change_a_product()
    {
        // Arrange
        Category initialCategory = Category.Create("Xiaomi");
        Category finalCategory = Category.Create("IPhone");
        Product initialProduct = Product.Create(
            "Xiaomi Mi A3",
            "64/8 gb",
            Price.Create(10_000),
            initialCategory.Id
        );

        using (ApplicationDbContext context = CreateDbContext())
        {
            await context.Categories.AddRangeAsync(initialCategory, finalCategory);
            await context.Products.AddAsync(initialProduct);
            await context.SaveChangesAsync();

            initialCategory.AddProductId(initialProduct.Id);
            await context.SaveChangesAsync();
        }

        var command = new ChangeProductCommand(
            initialProduct.Id.Value,
            "IPhone 13 pro",
            "512/12 gb",
            120_000,
            finalCategory.Id.Value
        );

        // Act
        Result result = await Sender.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();

        using (ApplicationDbContext context = CreateDbContext())
        {
            Category initialCategoryFromDatabase = (
                await context.Categories.FindAsync(initialCategory.Id)
            )!;

            initialCategoryFromDatabase.ProductIds.Count.Should().Be(0);

            Category finalCategoryFromDatabase = (
                await context.Categories.FindAsync(finalCategory.Id)
            )!;

            finalCategoryFromDatabase.ProductIds.Count.Should().Be(1);
            finalCategoryFromDatabase
                .ProductIds.Count(pi => pi == initialProduct.Id)
                .Should()
                .Be(1);

            Product finalProduct = (await context.Products.FindAsync(initialProduct.Id))!;

            finalProduct.Name.Should().Be("IPhone 13 pro");
            finalProduct.Description.Should().Be("512/12 gb");
            finalProduct.Price.Value.Should().Be(120_000);
            finalProduct.CategoryId.Should().Be(finalCategoryFromDatabase.Id);
        }
    }

    [Fact]
    public async Task Get_an_error_when_change_product_name_that_already_exists()
    {
        // Arrange
        Category category = Category.Create("Phones");

        Product storedProduct = Product.Create(
            "IPhone 13 pro",
            "512/12 gb",
            Price.Create(120_000),
            category.Id
        );
        Product changedProduct = Product.Create(
            "Xiaomi Mi A3",
            "64/12 gb",
            Price.Create(10_000),
            category.Id
        );

        using (ApplicationDbContext context = CreateDbContext())
        {
            await context.Categories.AddAsync(category);
            await context.Products.AddRangeAsync(storedProduct, changedProduct);
            await context.SaveChangesAsync();

            category.AddProductId(storedProduct.Id);
            category.AddProductId(changedProduct.Id);
            await context.SaveChangesAsync();
        }

        var command = new ChangeProductCommand(
            changedProduct.Id.Value,
            "IPhone 13 pro",
            "512/12 gb",
            120_000,
            category.Id.Value
        );

        // Act
        Result result = await Sender.Send(command);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Single().Message.Should().Be("Product with name already exists");

        using (ApplicationDbContext context = CreateDbContext())
        {
            context.Products.Count().Should().Be(2);

            Product changedProductFromDatabase = (
                await context.Products.FindAsync(changedProduct.Id)
            )!;

            changedProductFromDatabase.Name.Should().Be("Xiaomi Mi A3");
            changedProductFromDatabase.Description.Should().Be("64/12 gb");
            changedProductFromDatabase.Price.Value.Should().Be(10_000);
            changedProductFromDatabase.CategoryId.Should().Be(category.Id);

            Product storedProductFromDatabase = (
                await context.Products.FindAsync(storedProduct.Id)
            )!;

            storedProductFromDatabase.Name.Should().Be("IPhone 13 pro");
            storedProductFromDatabase.Description.Should().Be("512/12 gb");
            storedProductFromDatabase.Price.Value.Should().Be(120_000);
            storedProductFromDatabase.CategoryId.Should().Be(category.Id);
        }
    }

    [Fact]
    public async Task Get_an_error_when_change_product_category_id_that_does_not_exist()
    {
        // Arrange
        Category category = Category.Create("Phones");
        Product product = Product.Create(
            "IPhone 13 pro",
            "512/12 gb",
            Price.Create(120_000),
            category.Id
        );

        using (ApplicationDbContext context = CreateDbContext())
        {
            await context.Categories.AddAsync(category);
            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();

            category.AddProductId(product.Id);
            await context.SaveChangesAsync();
        }

        var command = new ChangeProductCommand(
            product.Id.Value,
            "Xiaomi Mi A3",
            "64/12 gb",
            10_000,
            Guid.NewGuid()
        );

        // Act
        Result result = await Sender.Send(command);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Single().Message.Should().Be("Category with id not found");

        using (ApplicationDbContext context = CreateDbContext())
        {
            Product productFromDatabase = (await context.Products.FindAsync(product.Id))!;

            productFromDatabase.Name.Should().Be("IPhone 13 pro");
            productFromDatabase.Description.Should().Be("512/12 gb");
            productFromDatabase.Price.Value.Should().Be(120_000);
            productFromDatabase.CategoryId.Should().Be(category.Id);

            Category categoryFromDatabase = (await context.Categories.FindAsync(category.Id))!;

            categoryFromDatabase.ProductIds.Count.Should().Be(1);
            categoryFromDatabase.ProductIds.Count(pi => pi == product.Id).Should().Be(1);
        }
    }

    [Fact]
    public async Task Get_an_error_when_change_product_that_does_not_exist()
    {
        // Arrange
        Category storedCategory = Category.Create("Phones");

        using (ApplicationDbContext context = CreateDbContext())
        {
            await context.Categories.AddAsync(storedCategory);
            await context.SaveChangesAsync();
        }

        var command = new ChangeProductCommand(
            Guid.NewGuid(),
            "IPhone 13 Pro",
            "512/12 gb",
            120_000,
            storedCategory.Id.Value
        );

        // Act
        Result result = await Sender.Send(command);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Single().Message.Should().Be("Product with id not found");

        using (ApplicationDbContext context = CreateDbContext())
        {
            context.Products.Count().Should().Be(0);

            Category categoryFromDatabase = (
                await context.Categories.FindAsync(storedCategory.Id)
            )!;

            categoryFromDatabase.ProductIds.Count.Should().Be(0);
        }
    }
}
