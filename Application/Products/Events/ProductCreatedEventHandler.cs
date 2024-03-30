using Domain.Category;
using Domain.Product.Events;
using Infrastructure.Data;
using MediatR;

namespace Application.Products.Events;

public class ProductCreatedEventHandler : INotificationHandler<ProductCreatedEvent>
{
    private readonly ApplicationDbContext _context;

    public ProductCreatedEventHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
    {
        Category category = (
            await _context.Categories.FindAsync([notification.CategoryId], cancellationToken)
        )!;

        category.AddProductId(notification.ProductId);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
