using FluentValidation;

namespace Application.Products.Commands.ChangeProduct;

public class ChangeProductCommandValidator : AbstractValidator<ChangeProductCommand>
{
    public ChangeProductCommandValidator()
    {
        RuleFor(p => p.ProductId).NotNull().NotEmpty();

        RuleFor(p => p.Name).NotNull().NotEmpty();

        RuleFor(p => p.Description).NotNull().NotEmpty();

        RuleFor(p => p.Price).Must(price => price > 0).WithMessage("Product price can't be null");

        RuleFor(p => p.CategoryId).NotNull().NotEmpty();
    }
}
