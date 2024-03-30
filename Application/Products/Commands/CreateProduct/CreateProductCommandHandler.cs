using Domain.Category.ValueObject;
using Domain.Product;
using Domain.Product.ValueObject;
using FluentResults;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Products.Commands.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<ProductId>>
{
    private readonly ApplicationDbContext _context;

    public CreateProductCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<ProductId>> Handle(
        CreateProductCommand request,
        CancellationToken cancellationToken
    )
    {
        if (
            await _context.Products.FirstOrDefaultAsync(
                p => p.Name == request.Name,
                cancellationToken
            ) != null
        )
        {
            return Result.Fail<ProductId>("Product with name already exists");
        }

        if (
            await _context.Categories.FindAsync(
                [CategoryId.Create(request.CategoryId)],
                cancellationToken
            ) == null
        )
        {
            return Result.Fail<ProductId>("Category with id not found");
        }

        Product product = Product.Create(
            request.Name,
            request.Description,
            Price.Create(request.Price),
            CategoryId.Create(request.CategoryId)
        );

        await _context.Products.AddAsync(product, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return product.Id;
    }
}
