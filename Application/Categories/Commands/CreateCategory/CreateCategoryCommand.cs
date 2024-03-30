using Domain.Category.ValueObject;
using FluentResults;
using MediatR;

namespace Application.Categories.Commands.CreateCategory;

public record CreateCategoryCommand(string Name) : IRequest<Result<CategoryId>>;
