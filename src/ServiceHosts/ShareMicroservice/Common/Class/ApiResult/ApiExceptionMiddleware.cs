using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;


namespace ShareMicroservice.Common.Class.ApiResult;


public class ApiExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ApiExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ApiException ex)
        {
            await HandleExceptionAsync(context, ex.Message, ex.StatusCode);
        }
        catch (UnauthorizedAccessException ex)
        {
            await HandleExceptionAsync(context, ex.Message, (int)HttpStatusCode.Unauthorized);
        }
        catch (Exception)
        {
            await HandleExceptionAsync(context, "Internal Server Error",
                (int)HttpStatusCode.InternalServerError);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, string message, int statusCode)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var response = ApiResult.Failure(message, statusCode);

        var json = JsonSerializer.Serialize(response);

        await context.Response.WriteAsync(json);
    }
}
