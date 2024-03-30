using Domain.Category.ValueObject;
using Domain.Common.Models;
using Domain.Product.ValueObject;

namespace Domain.Category;

public class Category : AggregateRoot<CategoryId>
{
    private readonly List<ProductId> _productIds = new List<ProductId>();

    public string Name { get; }
    public IReadOnlyList<ProductId> ProductIds => _productIds.AsReadOnly();

    private Category()
        : base(CategoryId.CreateUnique()) { }

    private Category(CategoryId categoryId, string name)
        : base(categoryId)
    {
        Name = name;
    }

    public static Category Create(string name)
    {
        return new Category(CategoryId.CreateUnique(), name);
    }

    public void AddProductId(ProductId productId)
    {
        _productIds.Add(productId);
    }

    public void RemoveProductId(ProductId productId)
    {
        _productIds.Remove(productId);
    }
}
