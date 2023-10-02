using FluentValidation;
using Products.Requests;

namespace Products.Validators;

public class GetProductByIdRequestValidator : AbstractValidator<GetProductByIdRequest>
{
    public GetProductByIdRequestValidator()
    {
        RuleFor(request => request.ProductId)
            .GreaterThan(0);
    }
}