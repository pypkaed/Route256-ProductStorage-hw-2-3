using FluentValidation;
using Products.Requests;

namespace Products.Validators;

public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
{
    private const long MinProductId = 0;
    private const decimal MinProductPrice = 0;
    private const double MinProductWeight = 0;
    private const long MinWarehouseId = 0;

    public CreateProductRequestValidator()
    {
        RuleFor(product => product.Id)
            .NotNull()
            .WithMessage("Product must have an id")
            .GreaterThan(MinProductId)
            .WithMessage($"Product id must be greater than {MinProductId}");
        RuleFor(product => product.Name)
            .NotNull()
            .NotEmpty()
            .WithMessage("Product must have a name");
        RuleFor(product => product.Price)
            .NotNull()
            .WithMessage("Product must have a price")
            .GreaterThan(MinProductPrice)
            .WithMessage($"Product price must be greater than {MinProductPrice}");
        RuleFor(product => product.Weight)
            .NotNull()
            .WithMessage("Product must have weight")
            .GreaterThan(MinProductWeight)
            .WithMessage($"Product weight must be greater than {MinProductWeight}");
        RuleFor(product => product.Category)
            .NotNull()
            .WithMessage("Product must have a category");
        RuleFor(product => product.ManufactureDate)
            .NotNull()
            .WithMessage("Product must have a manufacture date");
        RuleFor(product => product.WarehouseId)
            .NotNull()
            .WithMessage("Product must have a warehouse id")
            .GreaterThan(MinWarehouseId)
            .WithMessage($"Warehouse id must be greater than {MinWarehouseId}");
    }
}