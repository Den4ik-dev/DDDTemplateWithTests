using FluentValidation;

namespace Application.Authentication.Commands.RefreshToken;

public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(token => token.AccessToken).NotNull().NotEmpty();

        RuleFor(token => token.RefreshToken).NotNull().NotEmpty();
    }
}
