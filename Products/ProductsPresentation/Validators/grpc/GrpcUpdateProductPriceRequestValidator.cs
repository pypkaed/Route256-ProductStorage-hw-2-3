using FluentValidation;
using Products.Requests;

namespace Products.Validators.grpc;

public class GrpcUpdateProductPriceRequestValidator : AbstractValidator<ProductGrpc.UpdateProductPriceRequest>
{
    public GrpcUpdateProductPriceRequestValidator()
    {
        RuleFor(product => product.Id)
            .GreaterThan(0);
        RuleFor(product => product.Price)
            .GreaterThan(0);
    }
}