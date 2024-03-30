using Domain.Category.ValueObject;
using Domain.Common.Models;
using Domain.Product.Events;
using Domain.Product.ValueObject;

namespace Domain.Product;

public class Product : AggregateRoot<ProductId>
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Price Price { get; private set; }
    public CategoryId CategoryId { get; private set; }

    private Product()
        : base(ProductId.CreateUnique()) { }

    private Product(
        ProductId productId,
        string name,
        string description,
        Price price,
        CategoryId categoryId
    )
        : base(productId)
    {
        Name = name;
        Description = description;
        Price = price;
        CategoryId = categoryId;
    }

    public static Product Create(
        string name,
        string description,
        Price price,
        CategoryId categoryId
    )
    {
        var product = new Product(ProductId.CreateUnique(), name, description, price, categoryId);

        product.AddDomainEvent(new ProductCreatedEvent(product.Id, product.CategoryId));

        return product;
    }

    public void Change(string name, string description, Price price, CategoryId newCategoryId)
    {
        Name = name;
        Description = description;
        Price = price;

        if (CategoryId != newCategoryId)
        {
            AddDomainEvent(new ProductCategoryIdChangedEvent(Id, CategoryId, newCategoryId));

            CategoryId = newCategoryId;
        }
    }
}
