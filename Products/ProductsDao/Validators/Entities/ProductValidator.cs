using FluentValidation;
using ProductsDao.Entities;

namespace ProductsDao.Validators.Entities;

public class ProductValidator : AbstractValidator<Product>
{
    public ProductValidator()
    {
        RuleFor(product => product.Id)
            .NotNull()
            .WithMessage("Product must have an id");
        RuleFor(product => product.Name)
            .NotNull()
            .WithMessage("Product must have a name");
        RuleFor(product => product.Price)
            .NotNull()
            .WithMessage("Product must have a price");
        RuleFor(product => product.Weight)
            .NotNull()
            .WithMessage("Product must have weight");
        RuleFor(product => product.Category)
            .NotNull()
            .WithMessage("Product must have a category");
        RuleFor(product => product.ManufactureDate)
            .NotNull()
            .WithMessage("Product must have a manufacture date");
        RuleFor(product => product.WarehouseId)
            .NotNull()
            .WithMessage("Product must have a warehouse id");
    }
}