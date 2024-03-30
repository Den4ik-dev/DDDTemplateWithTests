using Domain.Product.ValueObject;
using FluentResults;
using MediatR;

namespace Application.Products.Commands.CreateProduct;

public record CreateProductCommand(string Name, string Description, int Price, Guid CategoryId)
    : IRequest<Result<ProductId>>;
