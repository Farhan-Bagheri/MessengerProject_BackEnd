using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ShareMicroservice.Common.Api;

public class ApiResult
{
    public bool IsSuccess { get; set; }
    public int StatusCode { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Message { get; set; }

    public ApiResult()
    {
    }

    public ApiResult(bool isSuccess, ApiResultStatusCode statusCode, string message = "")
    {
        IsSuccess = isSuccess;
        StatusCode = (int)statusCode;
        Message = message ?? statusCode.ToString();
    }

    #region Implicit Operators

    public static implicit operator ApiResult(OkResult result)
    {
        return new ApiResult(true, ApiResultStatusCode.Success, ApiResultStatusCode.Success.ToDisplay());
    }

    /// <summary>
    /// message in ApiResult Fille Value in OkObjectResult
    /// </summary>
    /// <param name="result"></param>
    public static implicit operator ApiResult(OkObjectResult result)
    {
        if (result.Value != null)
        {
            return new ApiResult(true, ApiResultStatusCode.Success, message: result.Value?.ToString());
        }

        return new ApiResult(true, ApiResultStatusCode.Success);
    }

    public static implicit operator ApiResult(BadRequestResult result)
    {
        return new ApiResult(false, ApiResultStatusCode.BadRequest, ApiResultStatusCode.BadRequest.ToDisplay());
    }

    public static implicit operator ApiResult(BadRequestObjectResult result)
    {
        var message = result.Value?.ToString();
        if (result.Value is SerializableError errors)
        {
            var errorMessages = errors.SelectMany(p => (string[])p.Value).Distinct();
            message = string.Join(" | ", errorMessages);
        }

        return new ApiResult(false, ApiResultStatusCode.BadRequest, message);
    }

    public static implicit operator ApiResult(ContentResult result)
    {
        return new ApiResult(true, ApiResultStatusCode.Success, result.Content);
    }

    public static implicit operator ApiResult(NotFoundResult result)
    {
        return new ApiResult(false, ApiResultStatusCode.NotFound);
    }

    public static implicit operator ApiResult(NotFoundObjectResult result)
    {
        return new ApiResult(false, ApiResultStatusCode.NotFound);
    }

    #endregion
}

public class ApiResult<TData> : ApiResult where TData : class
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public TData Data { get; set; }

    public ApiResult()
    {
    }

    public ApiResult(bool isSuccess, ApiResultStatusCode statusCode, TData data, string message = "")
        : base(isSuccess, statusCode, message)
    {
        IsSuccess = isSuccess;
        StatusCode = (int)statusCode;
        Message = string.IsNullOrWhiteSpace(message) ? statusCode.ToDisplay() : message;

        Data = data;
    }

    #region Implicit Operators

    public static implicit operator ApiResult<TData>(TData data)
    {
        return new ApiResult<TData>(true, ApiResultStatusCode.Success, data);
    }

    public static implicit operator ApiResult<TData>(OkResult result)
    {
        return new ApiResult<TData>(true, ApiResultStatusCode.Success, null);
    }

    public static implicit operator ApiResult<TData>(OkObjectResult result)
    {
        return new ApiResult<TData>(true, ApiResultStatusCode.Success, (TData)result.Value);
    }

    public static implicit operator ApiResult<TData>(BadRequestResult result)
    {
        return new ApiResult<TData>(false, ApiResultStatusCode.BadRequest, null);
    }

    public static implicit operator ApiResult<TData>(BadRequestObjectResult result)
    {
        var message = result.Value?.ToString();
        if (result.Value is string err)
        {
            message = err;
            return new ApiResult<TData>(false, ApiResultStatusCode.BadRequest, null, message);
        }

        return new ApiResult<TData>(false, ApiResultStatusCode.BadRequest, (TData)result.Value, message);
    }

    public static implicit operator ApiResult<TData>(ContentResult result)
    {
        return new ApiResult<TData>(true, ApiResultStatusCode.Success, null, result.Content);
    }

    public static implicit operator ApiResult<TData>(NotFoundResult result)
    {
        return new ApiResult<TData>(false, ApiResultStatusCode.NotFound, null);
    }

    public static implicit operator ApiResult<TData>(NotFoundObjectResult result)
    {
        return new ApiResult<TData>(false, ApiResultStatusCode.NotFound, (TData)result.Value);
    }

    public static implicit operator ApiResult<TData>(JsonResult result)
    {
        if (result.Value is ApiResult<TData> data)
        {
            return new ApiResult<TData>(data.IsSuccess, (ApiResultStatusCode)data.StatusCode, data.Data, data.Message);
        }

        return new ApiResult<TData>(false, ApiResultStatusCode.ServerError, null, ApiResultStatusCode.ServerError.ToDisplay());
    }

    #endregion
}