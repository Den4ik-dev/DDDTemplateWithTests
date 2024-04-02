using FluentValidation;

namespace Application.Authentication.Commands.Registration;

public class RegistrationCommandValidator : AbstractValidator<RegistrationCommand>
{
    public RegistrationCommandValidator()
    {
        RuleFor(regUser => regUser.Login).NotNull().NotEmpty();

        RuleFor(regUser => regUser.Password).NotNull().NotEmpty().MinimumLength(8);
    }
}
