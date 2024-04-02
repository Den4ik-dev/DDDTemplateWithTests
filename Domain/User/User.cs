using Domain.Common.Models;
using Domain.User.Enum;
using Domain.User.ValueObject;

namespace Domain.User;

public class User : AggregateRoot<UserId>
{
    public string Login { get; }
    public Password Password { get; }
    public string Role { get; } = Roles.STANDARD_USER;

    public string? RefreshToken { get; private set; }
    public DateTime? RefreshTokenExpiryTime { get; private set; }

    private User()
        : base(UserId.CreateUnique()) { }

    private User(UserId userId, string login, Password password)
        : base(userId)
    {
        Login = login;
        Password = password;
    }

    public static User Create(string login, string password)
    {
        return new User(UserId.CreateUnique(), login, Password.CreateHash(password));
    }

    public void SetRefreshToken(string refreshToken, DateTime refreshTokenExpiryTime)
    {
        RefreshToken = refreshToken;
        RefreshTokenExpiryTime = refreshTokenExpiryTime;
    }
}
