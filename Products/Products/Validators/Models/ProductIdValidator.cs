using FluentValidation;
using Products.Models;

namespace Products.Validators.Models;

public class ProductIdValidator : AbstractValidator<ProductId>
{
    private const long MinProductId = 0;
    
    public ProductIdValidator()
    {
        RuleFor(id => id.Id)
            .GreaterThan(MinProductId)
            .WithMessage($"Product id must be greater than {MinProductId}");
    }
}