using FluentResults;
using MediatR;

namespace Application.Products.Commands.ChangeProduct;

public record ChangeProductCommand(
    Guid ProductId,
    string Name,
    string Description,
    int Price,
    Guid CategoryId
) : IRequest<Result>;
