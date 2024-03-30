using Domain.Category;
using Domain.Product.Events;
using Infrastructure.Data;
using MediatR;

namespace Application.Products.Events;

public class ProductCategoryIdChangedEventHandler
    : INotificationHandler<ProductCategoryIdChangedEvent>
{
    private readonly ApplicationDbContext _context;

    public ProductCategoryIdChangedEventHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        ProductCategoryIdChangedEvent notification,
        CancellationToken cancellationToken
    )
    {
        Category initialCategory = (
            await _context.Categories.FindAsync([notification.InitialCategoryId], cancellationToken)
        )!;

        await _context
            .Entry(initialCategory)
            .Collection(c => c.ProductIds)
            .LoadAsync(cancellationToken);

        initialCategory.RemoveProductId(notification.ProductId);
        await _context.SaveChangesAsync(cancellationToken);

        Category finalCategory = (
            await _context.Categories.FindAsync([notification.FinalCategoryId], cancellationToken)
        )!;

        finalCategory.AddProductId(notification.ProductId);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
