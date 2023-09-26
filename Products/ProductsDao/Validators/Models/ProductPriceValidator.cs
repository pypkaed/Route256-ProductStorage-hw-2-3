using FluentValidation;
using ProductsDao.Models;

namespace ProductsDao.Validators.Models;

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