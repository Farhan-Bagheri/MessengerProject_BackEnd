using Scalar.AspNetCore;

namespace GatewayApi.Config;

public static class ScalarExtensions
{
    public static IEndpointRouteBuilder UseCustomScalarApi(
        this IEndpointRouteBuilder app,
        IConfiguration configuration)
    {
        var services = configuration
            .GetSection("Services")
            .GetChildren()
            .ToList();

        var isFirst = true;

        app.MapScalarApiReference(options =>
        {
            foreach (var service in services)
            {
                var module = service.Key;

                var displayName =
                    service["DisplayName"] ?? $"{module} API";

                var openApiUrl =
                    $"/openapi/{module}/v1.json";

                options.AddDocument(
                    $"{module.ToLowerInvariant()}-v1",
                    $"{displayName} v1",
                    openApiUrl,
                    isDefault: isFirst);

                isFirst = false;
            }

            options.WithPreferredScheme("Bearer");
        });

        return app;
    }
}