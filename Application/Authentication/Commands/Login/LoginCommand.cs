using FluentResults;
using Infrastructure.Authentication;
using MediatR;

namespace Application.Authentication.Commands.Login;

public record LoginCommand(string Login, string Password) : IRequest<Result<Token>>;
