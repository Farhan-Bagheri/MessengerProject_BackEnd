namespace ShopApi.Config;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseGlobalException(this IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }
}