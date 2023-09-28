using FluentValidation;
using Products.Requests;

namespace Products.Validators;

public class DeleteProductByIdRequestValidator : AbstractValidator<DeleteProductByIdRequest>
{
    private const long MinProductId = 0;
    
    public DeleteProductByIdRequestValidator()
    {
        RuleFor(request => request.ProductId)
            .NotNull()
            .WithMessage("Product must have an id")
            .GreaterThan(MinProductId)
            .WithMessage($"Product id must be greater than {MinProductId}");
    }
}