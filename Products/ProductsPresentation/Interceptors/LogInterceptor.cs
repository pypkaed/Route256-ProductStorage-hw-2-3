using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Products.Interceptors;

public class LogInterceptor : Interceptor
{
    private readonly ILogger<LogInterceptor> _logger;

    public LogInterceptor(ILogger<LogInterceptor> logger)
    {
        _logger = logger;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        var traceId = Guid.NewGuid();
        
        _logger.LogInformation($"{traceId}: Starting {context.Method} with request {request}");
        var response = await continuation(request, context);
        _logger.LogInformation($"{traceId}: End of {context.Method} with request {request}, response is {response}");
        
        return response;
    }
}