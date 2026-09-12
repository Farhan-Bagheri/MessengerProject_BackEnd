using System.Diagnostics;

namespace GatewayApi.Middleware;

public sealed class RequestLoggingMiddleware(
    RequestDelegate next,
    ILogger<RequestLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        var method = context.Request.Method;
        var path = context.Request.Path.Value ?? "/";
        var queryString = context.Request.QueryString.Value;

        var correlationId =
            context.Items["X-Correlation-ID"]?.ToString()
            ?? "unknown";

        try
        {
            logger.LogInformation(
                "Gateway request started | {Method} {Path}{QueryString} | CorrelationId: {CorrelationId} | RemoteIp: {RemoteIp}",
                method,
                path,
                queryString,
                correlationId,
                context.Connection.RemoteIpAddress);

            await next(context);

            stopwatch.Stop();

            logger.LogInformation(
                "Gateway request completed | {Method} {Path} | StatusCode: {StatusCode} | Duration: {DurationMs}ms | CorrelationId: {CorrelationId}",
                method,
                path,
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds,
                correlationId);
        }
        catch (Exception exception)
        {
            stopwatch.Stop();

            logger.LogError(
                exception,
                "Gateway request failed | {Method} {Path} | Duration: {DurationMs}ms | CorrelationId: {CorrelationId}",
                method,
                path,
                stopwatch.ElapsedMilliseconds,
                correlationId);

            throw;
        }
    }
}