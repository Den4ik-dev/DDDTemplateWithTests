using FluentResults;
using Infrastructure.Authentication;
using MediatR;

namespace Application.Authentication.Commands.RefreshToken;

public record RefreshTokenCommand(string AccessToken, string RefreshToken)
    : IRequest<Result<Token>>;
