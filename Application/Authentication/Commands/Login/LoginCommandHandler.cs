using Domain.User;
using FluentResults;
using Infrastructure.Authentication;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Authentication.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<Token>>
{
    private readonly ApplicationDbContext _context;
    private readonly JwtTokenGenerator _jwtTokenGenerator;

    public LoginCommandHandler(ApplicationDbContext context, JwtTokenGenerator jwtTokenGenerator)
    {
        _context = context;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<Result<Token>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken
    )
    {
        User? user = await _context.Users.FirstOrDefaultAsync(
            u => u.Login == request.Login,
            cancellationToken
        );

        if (user is null || !user.Password.PasswordVerify(request.Password))
        {
            return Result.Fail<Token>("User with login and password not found");
        }

        Token token = _jwtTokenGenerator.GenerateToken(user);

        user.SetRefreshToken(token.RefreshToken, DateTime.UtcNow.AddDays(15));
        await _context.SaveChangesAsync(cancellationToken);

        return token;
    }
}
