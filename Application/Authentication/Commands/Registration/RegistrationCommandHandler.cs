using System.Security.Claims;
using Domain.User;
using FluentResults;
using Infrastructure.Authentication;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Authentication.Commands.Registration;

public class RegistrationCommandHandler : IRequestHandler<RegistrationCommand, Result<Token>>
{
    private readonly ApplicationDbContext _context;
    private readonly JwtTokenGenerator _jwtTokenGenerator;

    public RegistrationCommandHandler(
        ApplicationDbContext context,
        JwtTokenGenerator jwtTokenGenerator
    )
    {
        _context = context;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<Result<Token>> Handle(
        RegistrationCommand request,
        CancellationToken cancellationToken
    )
    {
        if (
            await _context.Users.FirstOrDefaultAsync(
                u => u.Login == request.Login,
                cancellationToken
            ) != null
        )
        {
            return Result.Fail<Token>("User with login already exists");
        }

        User user = User.Create(request.Login, request.Password);

        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        Token token = _jwtTokenGenerator.GenerateToken(user);

        user.SetRefreshToken(token.RefreshToken, DateTime.UtcNow.AddDays(15));
        await _context.SaveChangesAsync(cancellationToken);

        return token;
    }
}
