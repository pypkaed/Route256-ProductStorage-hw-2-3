using FluentValidation;
using Products.Requests;

namespace Products.Validators;

public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductRequestValidator()
    {
        RuleFor(product => product.Id)
            .GreaterThan(0);
        RuleFor(product => product.Name)
            .NotNull()
            .NotEmpty();
        RuleFor(product => product.Price)
            .GreaterThan(0);
        RuleFor(product => product.Weight)
            .GreaterThan(0);
        RuleFor(product => product.Category)
            .NotNull();
        RuleFor(product => product.ManufactureDate)
            .NotNull();
        RuleFor(product => product.WarehouseId)
            .GreaterThan(0);
    }
}