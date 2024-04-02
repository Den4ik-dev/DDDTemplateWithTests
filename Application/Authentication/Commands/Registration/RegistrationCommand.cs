using FluentResults;
using Infrastructure.Authentication;
using MediatR;

namespace Application.Authentication.Commands.Registration;

public record RegistrationCommand(string Login, string Password) : IRequest<Result<Token>>;
