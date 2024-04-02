using Domain.User;
using Domain.User.ValueObject;
using FluentResults;
using Infrastructure.Authentication;
using Infrastructure.Data;
using MediatR;

namespace Application.Authentication.Commands.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<Token>>
{
    private readonly ApplicationDbContext _context;
    private readonly JwtTokenGenerator _jwtTokenGenerator;

    public RefreshTokenCommandHandler(
        ApplicationDbContext context,
        JwtTokenGenerator jwtTokenGenerator
    )
    {
        _context = context;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<Result<Token>> Handle(
        RefreshTokenCommand request,
        CancellationToken cancellationToken
    )
    {
        Result<UserId> result = _jwtTokenGenerator.GetUserIdFromExpiredToken(request.AccessToken);

        if (result.IsFailed)
        {
            return Result.Fail<Token>(result.Errors);
        }

        UserId userId = result.Value;
        User? user = await _context.Users.FindAsync([userId], cancellationToken);

        if (
            user is null
            || user.RefreshToken != request.RefreshToken
            || user.RefreshTokenExpiryTime <= DateTime.UtcNow
        )
        {
            return Result.Fail<Token>("Refresh token is invalid");
        }

        Token token = _jwtTokenGenerator.GenerateToken(user);

        user.SetRefreshToken(token.RefreshToken, DateTime.Now.AddDays(15));
        await _context.SaveChangesAsync(cancellationToken);

        return token;
    }
}
