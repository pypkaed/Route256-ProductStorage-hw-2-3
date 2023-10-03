using FluentValidation;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Products.Interceptors;

public class ValidationInterceptor : Interceptor
{
    private readonly IServiceProvider _serviceProvider;

    public ValidationInterceptor(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        var validator = _serviceProvider.GetService<IValidator<TRequest>>();
        if (validator is not null)
        {
            await validator.ValidateAndThrowAsync(request);
        }

        return await continuation(request, context);
    }
}