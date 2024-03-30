namespace Domain.Category.ValueObject;

public class CategoryId : Common.Models.ValueObject
{
    public Guid Value { get; }

    private CategoryId(Guid value)
    {
        Value = value;
    }

    public static CategoryId CreateUnique()
    {
        return new CategoryId(Guid.NewGuid());
    }

    public static CategoryId Create(Guid value)
    {
        return new CategoryId(value);
    }

    public override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
