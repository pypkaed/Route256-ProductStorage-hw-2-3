using FluentValidation;
using Products.Models;

namespace Products.Validators.Models;

public class ProductWeightValidator : AbstractValidator<ProductWeight>
{
    private const double MinProductWeight = 0;
    
    public ProductWeightValidator()
    {
        RuleFor(weight => weight.Weight)
            .GreaterThan(MinProductWeight)
            .WithMessage($"Product weight must be greater than {MinProductWeight}");
    }
}