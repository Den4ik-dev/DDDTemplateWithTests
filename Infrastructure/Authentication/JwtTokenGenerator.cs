using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Domain.User;
using Domain.User.ValueObject;
using FluentResults;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Authentication;

public class JwtTokenGenerator
{
    private readonly JwtSettings _jwtSettings;

    public JwtTokenGenerator(IOptions<JwtSettings> jwtSettingsOptions)
    {
        _jwtSettings = jwtSettingsOptions.Value;
    }

    private string GenerateRefreshToken()
    {
        byte[] randomNumbers = new byte[32];

        using RandomNumberGenerator rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumbers);

        return Convert.ToBase64String(randomNumbers);
    }

    private string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
            SecurityAlgorithms.HmacSha256
        );

        var jwt = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            expires: DateTime.UtcNow.AddMinutes(15),
            claims: claims,
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    public Token GenerateToken(User user)
    {
        string accessToken = GenerateAccessToken(
            [
                new Claim(ClaimTypes.Name, user.Id.Value.ToString()),
                new Claim(ClaimTypes.Role, user.Role.Value)
            ]
        );
        string refreshToken = GenerateRefreshToken();

        var token = new Token(accessToken, refreshToken);

        return token;
    }

    public Result<UserId> GetUserIdFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters()
        {
            ValidIssuer = _jwtSettings.Issuer,
            ValidAudience = _jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(_jwtSettings.Secret)
            ),

            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = false,
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        ClaimsPrincipal principal = tokenHandler.ValidateToken(
            token,
            tokenValidationParameters,
            out SecurityToken securityToken
        );

        if (
            securityToken is not JwtSecurityToken jwtSecurityToken
            || !jwtSecurityToken.Header.Alg.Equals(
                SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase
            )
        )
        {
            return Result.Fail<UserId>("Invalid access token");
        }

        UserId userId = UserId.Create(Guid.Parse(principal.Identity!.Name!));

        return userId;
    }
}
