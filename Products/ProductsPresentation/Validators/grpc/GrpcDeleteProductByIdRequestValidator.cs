using FluentValidation;

namespace Products.Validators.grpc;

public class GrpcDeleteProductByIdRequestValidator : AbstractValidator<ProductGrpc.DeleteProductByIdRequest>
{
    public GrpcDeleteProductByIdRequestValidator()
    {
        RuleFor(request => request.Id)
            .GreaterThan(0);
    }
}