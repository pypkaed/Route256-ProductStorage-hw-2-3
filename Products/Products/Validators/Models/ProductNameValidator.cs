using FluentValidation;
using Products.Models;

namespace Products.Validators.Models;

public class ProductNameValidator : AbstractValidator<ProductName>
{
    public ProductNameValidator()
    {
        RuleFor(name => name.Name)
            .NotNull()
            .NotEmpty()
            .WithMessage("Product name must not be empty or null");
    }
}