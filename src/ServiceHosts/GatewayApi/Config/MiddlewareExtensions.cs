using GatewayApi.Middleware;

namespace GatewayApi.Config;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseGatewayMiddleware(
        this IApplicationBuilder builder)
    {
        builder.UseMiddleware<CorrelationIdMiddleware>();
        builder.UseMiddleware<RequestLoggingMiddleware>();

        return builder;
    }
}