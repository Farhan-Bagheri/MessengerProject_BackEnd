using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace GatewayApi.Config;

internal static class GatewayProxyMetrics
{
    public const string MeterName = "GatewayApi.Proxy";

    private static readonly Meter Meter = new(MeterName);

    public static readonly Counter<long> Requests =
        Meter.CreateCounter<long>(
            "gateway_proxy_requests_total",
            unit: "{request}",
            description: "Total number of requests forwarded by YARP.");

    public static readonly Histogram<double> Duration =
        Meter.CreateHistogram<double>(
            "gateway_proxy_request_duration_seconds",
            unit: "s",
            description: "Duration of requests forwarded by YARP.");
}

internal static class GatewayProxyMetricsMiddleware
{
    public static async Task InvokeAsync(
        HttpContext context,
        Func<Task> next)
    {
        var startedAt = Stopwatch.GetTimestamp();

        var proxyFeature = context.GetReverseProxyFeature();

        var cluster =
            proxyFeature.Cluster?.Config.ClusterId
            ?? "unknown";

        var destination =
            proxyFeature.ProxiedDestination?.DestinationId
            ?? "unknown";

        var method = context.Request.Method;

        try
        {
            await next();
        }
        finally
        {
            var tags = new TagList
            {
                { "cluster", cluster },
                { "destination", destination },
                { "http.request.method", method },
                { "http.response.status_code", context.Response.StatusCode }
            };

            GatewayProxyMetrics.Requests.Add(1, tags);

            GatewayProxyMetrics.Duration.Record(
                Stopwatch.GetElapsedTime(startedAt).TotalSeconds,
                tags);
        }
    }
}