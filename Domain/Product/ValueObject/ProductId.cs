namespace Domain.Product.ValueObject;

public class ProductId : Common.Models.ValueObject
{
    public Guid Value { get; }

    private ProductId(Guid value)
    {
        Value = value;
    }

    public static ProductId CreateUnique()
    {
        return new ProductId(Guid.NewGuid());
    }

    public static ProductId Create(Guid value)
    {
        return new ProductId(value);
    }

    public override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
