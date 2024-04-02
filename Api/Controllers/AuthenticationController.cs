using Api.Common;
using Application.Authentication.Commands.Login;
using Application.Authentication.Commands.RefreshToken;
using Application.Authentication.Commands.Registration;
using Contracts.Authentication;
using FluentResults;
using Infrastructure.Authentication;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/auth")]
public class AuthenticationController : ApiController
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public AuthenticationController(ISender sender, IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    [HttpPost("login")]
    public async Task<IResult> Login(LoginDto loginDto)
    {
        LoginCommand command = _mapper.Map<LoginCommand>(loginDto);

        Result<Token> result = await _sender.Send(command);

        if (result.IsFailed)
        {
            return Problem(result.Errors);
        }

        Token token = result.Value;

        return Results.Ok(token);
    }

    [HttpPost("reg")]
    public async Task<IResult> Registration(RegistrationDto registrationDto)
    {
        RegistrationCommand command = _mapper.Map<RegistrationCommand>(registrationDto);

        Result<Token> result = await _sender.Send(command);

        if (result.IsFailed)
        {
            return Problem(result.Errors);
        }

        Token token = result.Value;

        return Results.Ok(token);
    }

    [HttpPost("token/refresh")]
    public async Task<IResult> RefreshToken(RefreshTokenDto refreshTokenDto)
    {
        RefreshTokenCommand command = _mapper.Map<RefreshTokenCommand>(refreshTokenDto);

        Result<Token> result = await _sender.Send(command);

        if (result.IsFailed)
        {
            return Problem(result.Errors);
        }

        Token token = result.Value;

        return Results.Ok(token);
    }
}
