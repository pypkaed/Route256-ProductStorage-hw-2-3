using FluentValidation;
using Products.Models;

namespace Products.Validators.Models;

public class ProductPriceValidator : AbstractValidator<ProductPrice>
{
    private const decimal MinProductPrice = 0;
    
    public ProductPriceValidator()
    {
        RuleFor(price => price.Price)
            .GreaterThan(MinProductPrice)
            .WithMessage($"Product price must be greater than {MinProductPrice}");
    }
}