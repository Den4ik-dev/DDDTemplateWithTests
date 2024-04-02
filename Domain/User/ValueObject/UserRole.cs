using ArgumentException = System.ArgumentException;

namespace Domain.User.ValueObject;

public class UserRole : Common.Models.ValueObject
{
    public string Value { get; }

    private UserRole(string value)
    {
        Value = value;
    }

    public override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public static UserRole CreateStandardUserRole()
    {
        return new UserRole(Roles.StandardUser);
    }

    public static UserRole CreateAdminRole()
    {
        return new UserRole(Roles.Admin);
    }

    public static UserRole Create(string value)
    {
        if (
            typeof(Roles)
                .GetFields()
                .All(constants => constants.GetRawConstantValue()!.ToString() != value)
        )
        {
            throw new ArgumentException($"Role name '{value}' not found");
        }

        return new UserRole(value);
    }
}

public static class Roles
{
    public const string Admin = "admin";
    public const string StandardUser = "standard_user";
}
