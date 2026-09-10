using Microsoft.AspNetCore.Mvc;
using ShareMicroservice.Common.Class.ApiResult;

namespace IdentityApi.Controllers;

[ApiController]
public class BaseController : ControllerBase
{
    public string? GetCurrentUserId
    {
        get
        {
            try
            {
                if (User is { Identity.IsAuthenticated: true })
                {
                    string? userId = User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value;
                    return string.IsNullOrWhiteSpace(userId) ? null : userId;
                }

                return null;
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }
    }

    protected ApiResult<T> SuccessResult<T>(T data)
    {
        Response.StatusCode = 200;
        return ApiResult<T>.SuccessResult(data);
    }

    // اگر برای خطاها لیست دارید، همیشه از این استفاده کنید
    protected ApiResult<T> BadRequestResult<T>(string? message, List<string>? errors = null)
    {
        return ApiResult<T>.Failure(message, errors);
    }

    // اگر فقط یک پیام ساده دارید
    protected ApiResult BadRequestResult(string? message = null)
    {
        return ApiResult.Failure(message, null);
    }

}