using System.Security.Cryptography;
using System.Text;

namespace Domain.User.ValueObject;

public class Password : Common.Models.ValueObject
{
    public string Value { get; }

    private Password(string value)
    {
        Value = value;
    }

    public static Password Create(string passwordHash)
    {
        return new Password(passwordHash);
    }

    public static Password CreateHash(string password)
    {
        string passwordHash = Convert.ToHexString(
            SHA512.Create().ComputeHash(Encoding.UTF8.GetBytes(password))
        );

        return new Password(passwordHash);
    }

    public bool PasswordVerify(string password)
    {
        string passwordHash = Convert.ToHexString(
            SHA512.Create().ComputeHash(Encoding.UTF8.GetBytes(password))
        );

        return Value.Equals(passwordHash);
    }

    public override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
