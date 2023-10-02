using FluentValidation;
using Products.Requests;

namespace Products.Validators.grpc;

public class GrpcGetProductByIdRequestValidator : AbstractValidator<ProductGrpc.GetProductByIdRequest>
{
    public GrpcGetProductByIdRequestValidator()
    {
        RuleFor(request => request.Id)
            .GreaterThan(0);
    }
}