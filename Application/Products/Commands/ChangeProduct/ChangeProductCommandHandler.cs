using Domain.Category.ValueObject;
using Domain.Product;
using Domain.Product.ValueObject;
using FluentResults;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Products.Commands.ChangeProduct;

public class ChangeProductCommandHandler : IRequestHandler<ChangeProductCommand, Result>
{
    private readonly ApplicationDbContext _context;

    public ChangeProductCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(
        ChangeProductCommand request,
        CancellationToken cancellationToken
    )
    {
        Product? product = await _context.Products.FindAsync(
            [ProductId.Create(request.ProductId)],
            cancellationToken
        );

        if (product is null)
        {
            return Result.Fail("Product with id not found");
        }

        if (
            await _context.Products.FirstOrDefaultAsync(
                p => p.Name == request.Name && p.Id != product.Id,
                cancellationToken
            ) != null
        )
        {
            return Result.Fail("Product with name already exists");
        }

        if (
            await _context.Categories.FindAsync(
                [CategoryId.Create(request.CategoryId)],
                cancellationToken
            ) == null
        )
        {
            return Result.Fail("Category with id not found");
        }

        product.Change(
            request.Name,
            request.Description,
            Price.Create(request.Price),
            CategoryId.Create(request.CategoryId)
        );

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}
