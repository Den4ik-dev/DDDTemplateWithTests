using Domain.Category;
using Domain.Category.ValueObject;
using FluentResults;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Categories.Commands.CreateCategory;

public class CreateCategoryCommandHandler
    : IRequestHandler<CreateCategoryCommand, Result<CategoryId>>
{
    private readonly ApplicationDbContext _context;

    public CreateCategoryCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<CategoryId>> Handle(
        CreateCategoryCommand request,
        CancellationToken cancellationToken
    )
    {
        if (
            await _context.Categories.FirstOrDefaultAsync(
                c => c.Name == request.Name,
                cancellationToken
            ) != null
        )
        {
            return Result.Fail<CategoryId>("Category with name already exists");
        }

        Category category = Category.Create(request.Name);

        await _context.Categories.AddAsync(category, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return category.Id;
    }
}
