using FluentValidation;
using Products.Models;

namespace Products.Validators.Models;

public class WarehouseIdValidator : AbstractValidator<WarehouseId>
{
    private const long MinWarehouseId = 0;
    
    public WarehouseIdValidator()
    {
        RuleFor(id => id.Id)
            .GreaterThan(MinWarehouseId)
            .WithMessage($"Warehouse id must be greater than {MinWarehouseId}");
    }
}