using FluentValidation;
using Products.Requests;

namespace Products.Validators;

public class DeleteProductByIdRequestValidator : AbstractValidator<DeleteProductByIdRequest>
{
    public DeleteProductByIdRequestValidator()
    {
        RuleFor(request => request.ProductId)
            .GreaterThan(0);
    }
}