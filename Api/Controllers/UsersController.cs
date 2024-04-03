using Api.Common;
using Application.Users.Queries.GetUserProfile;
using Contracts.Users;
using Domain.User;
using FluentResults;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/users")]
public class UsersController : ApiController
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public UsersController(ISender sender, IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet, Authorize]
    public async Task<IResult> GetUserProfile()
    {
        Guid userId = Guid.Parse(User.Identity!.Name!);
        var query = new GetUserProfileQuery(userId);

        Result<User> result = await _sender.Send(query);

        if (result.IsFailed)
        {
            return Problem(result.Errors);
        }

        User user = result.Value;
        UserProfileDto userProfile = _mapper.Map<UserProfileDto>(user);

        return Results.Ok(userProfile);
    }
}
