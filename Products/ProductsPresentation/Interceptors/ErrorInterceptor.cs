using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Products.Interceptors;

public class ErrorInterceptor : Interceptor
{
    private readonly ILogger<LogInterceptor> _logger;

    public ErrorInterceptor(ILogger<LogInterceptor> logger)
    {
        _logger = logger;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context);
        }
        catch (RpcException e)
        {
            _logger.LogError(e, $"Error while running {context.Method}. Error: {e.Message}");
            throw;
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, $"Error while running {context.Method}. Error: {e.Message}");
            throw;
        }
    }
}