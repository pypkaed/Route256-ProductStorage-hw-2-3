using FluentValidation;
using Products.Requests;

namespace Products.Validators;

public class UpdateProductPriceRequestValidator : AbstractValidator<UpdateProductPriceRequest>
{
    private const long MinProductId = 0;
    private const decimal MinProductPrice = 0;

    public UpdateProductPriceRequestValidator()
    {
        RuleFor(product => product.ProductId)
            .NotNull()
            .WithMessage("Product must have an id")
            .GreaterThan(MinProductId)
            .WithMessage($"Product id must be greater than {MinProductId}");
        RuleFor(product => product.NewPrice)
            .NotNull()
            .WithMessage("Product must have a price")
            .GreaterThan(MinProductPrice)
            .WithMessage($"Product price must be greater than {MinProductPrice}");
    }
}