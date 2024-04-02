using FluentValidation;

namespace Application.Authentication.Commands.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(loginUser => loginUser.Login).NotNull().NotEmpty();

        RuleFor(loginUser => loginUser.Password).NotNull().NotEmpty();
    }
}
