using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Products.Interceptors;

public class LogInterceptor : Interceptor
{
    private readonly Logger<LogInterceptor> _logger;

    public LogInterceptor(Logger<LogInterceptor> logger)
    {
        _logger = logger;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        _logger.LogInformation($"Starting {context.Method} with request {request}");
        var response = await continuation(request, context);
        _logger.LogInformation($"End of {context.Method} with request {request}, response is {response}");
        
        return response;
    }
}