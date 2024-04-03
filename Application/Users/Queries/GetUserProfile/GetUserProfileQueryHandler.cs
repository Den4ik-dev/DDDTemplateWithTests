using Domain.User;
using Domain.User.ValueObject;
using FluentResults;
using Infrastructure.Data;
using MediatR;

namespace Application.Users.Queries.GetUserProfile;

public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, Result<User>>
{
    private readonly ApplicationDbContext _context;

    public GetUserProfileQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<User>> Handle(
        GetUserProfileQuery request,
        CancellationToken cancellationToken
    )
    {
        User? user = await _context.Users.FindAsync(
            [UserId.Create(request.UserId)],
            cancellationToken
        );

        if (user is null)
        {
            return Result.Fail<User>("User with id not found");
        }

        return user;
    }
}
