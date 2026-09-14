using ShareMicroservice.Common.Class.ApiResult;

namespace ShopApi.Config;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseGlobalException(this IApplicationBuilder app)
    {
        app.UseMiddleware<ApiExceptionMiddleware>();
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }
}