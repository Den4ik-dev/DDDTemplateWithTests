using Application.Categories.Commands.CreateCategory;
using Application.IntegrationTests.Common;
using Domain.Category;
using Domain.Category.ValueObject;
using FluentAssertions;
using FluentResults;
using Infrastructure.Data;
using MediatR;

namespace Application.IntegrationTests.CategoriesTests;

public class CreateCategoryTests : BaseIntegrationTest
{
    public CreateCategoryTests(IntegrationTestWebApplicationFactory factory)
        : base(factory) { }

    [Fact]
    public async Task Get_a_category_id_when_creating_a_category_with_valid_data()
    {
        // Arrange
        var command = new CreateCategoryCommand("Phones");

        // Act
        Result<CategoryId> result = await Sender.Send(command);

        // Assert
        result.IsSuccess.Should().BeTrue();

        using (ApplicationDbContext context = CreateDbContext())
        {
            context.Categories.Count().Should().Be(1);

            Category category = (await context.Categories.FindAsync(result.Value))!;

            category.Name.Should().Be("Phones");
        }
    }

    [Fact]
    public async Task Get_an_error_when_creating_a_category_with_name_that_already_exists()
    {
        // Arrange
        using (ApplicationDbContext context = CreateDbContext())
        {
            Category category = Category.Create("Phones");

            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();
        }

        var command = new CreateCategoryCommand("Phones");

        // Act
        Result<CategoryId> result = await Sender.Send(command);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Single().Message.Should().Be("Category with name already exists");

        using (ApplicationDbContext context = CreateDbContext())
        {
            context.Categories.Count().Should().Be(1);
            context.Categories.Count(c => c.Name == "Phones").Should().Be(1);
        }
    }
}
