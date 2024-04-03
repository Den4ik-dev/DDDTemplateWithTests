using Domain.User;
using FluentResults;
using MediatR;

namespace Application.Users.Queries.GetUserProfile;

public record GetUserProfileQuery(Guid UserId) : IRequest<Result<User>>;
