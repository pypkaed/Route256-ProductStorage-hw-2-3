using FluentValidation;

namespace Products.Validators.grpc;

public class GrpcUpdateProductPriceRequestValidator : AbstractValidator<ProductGrpc.UpdateProductPriceRequest>
{
    public GrpcUpdateProductPriceRequestValidator()
    {
        RuleFor(product => product.Id)
            .GreaterThan(0);
        RuleFor(product => product.Price.Units)
            .GreaterThan(0);
        RuleFor(product => product.Price.Nanos)
            .GreaterThan(0);
    }
}