using FluentValidation;

namespace Application.Products.Commands.CreateProduct;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(p => p.Name).NotNull().NotEmpty();

        RuleFor(p => p.Description).NotNull().NotEmpty();

        RuleFor(p => p.Price).Must(price => price > 0).WithMessage("Product price can't be null");

        RuleFor(p => p.CategoryId).NotNull().NotEmpty();
    }
}
