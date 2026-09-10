using Scalar.AspNetCore;

namespace GatewayApi.Config;

public static class ApplicationBuilderExtensions
{
    public static IEndpointRouteBuilder UseCustomeScalarApi(
        this IEndpointRouteBuilder app)
    {
        app.MapScalarApiReference(options =>
        {
            options
                .AddDocument(
                    "identity-v1",
                    "Identity API v1",
                    "/openapi/identity/v1.json",
                    isDefault: true)

                .AddDocument(
                    "shop-v1",
                    "Shop API v1",
                    "/openapi/shop/v1.json");
        });

        return app;
    }
}