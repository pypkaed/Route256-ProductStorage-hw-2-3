using FluentValidation;
using Products.Requests;

namespace Products.Validators;

public class UpdateProductPriceRequestValidator : AbstractValidator<UpdateProductPriceRequest>
{
    public UpdateProductPriceRequestValidator()
    {
        RuleFor(product => product.ProductId)
            .GreaterThan(0);
        RuleFor(product => product.NewPrice)
            .GreaterThan(0);
    }
}