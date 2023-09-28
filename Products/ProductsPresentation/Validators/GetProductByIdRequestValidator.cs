using FluentValidation;
using Products.Requests;

namespace Products.Validators;

public class GetProductByIdRequestValidator : AbstractValidator<GetProductByIdRequest>
{
    private const long MinProductId = 0;

    public GetProductByIdRequestValidator()
    {
        RuleFor(request => request.ProductId)
            .NotNull()
            .WithMessage("Product must have an id")
            .GreaterThan(MinProductId)
            .WithMessage($"Product id must be greater than {MinProductId}");
    }
}