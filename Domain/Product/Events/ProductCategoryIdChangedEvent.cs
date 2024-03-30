using Domain.Category.ValueObject;
using Domain.Common.Models;
using Domain.Product.ValueObject;

namespace Domain.Product.Events;

public record ProductCategoryIdChangedEvent(
    ProductId ProductId,
    CategoryId InitialCategoryId,
    CategoryId FinalCategoryId
) : IDomainEvent;
