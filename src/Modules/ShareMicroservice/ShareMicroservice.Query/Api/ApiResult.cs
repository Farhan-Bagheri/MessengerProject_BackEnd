using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShareMicroservice.Query.Extentions;

namespace ShareMicroservice.Query.Api;

public class ApiResult
{
    public bool IsSuccess { get; set; }

    public int StatusCode { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Message { get; set; }

    public ApiResult()
    {
    }

    public ApiResult(
        bool isSuccess,
        ApiResultStatusCode statusCode,
        string message = "")
    {
        IsSuccess = isSuccess;
        StatusCode = (int)statusCode;
        Message = string.IsNullOrWhiteSpace(message)
            ? statusCode.ToDisplay()
            : message;
    }

    #region Implicit Operators

    public static implicit operator ApiResult(OkResult result)
    {
        return new ApiResult(
            true,
            ApiResultStatusCode.Success);
    }

    public static implicit operator ApiResult(OkObjectResult result)
    {
        return new ApiResult(
            true,
            ApiResultStatusCode.Success,
            result.Value?.ToString());
    }

    public static implicit operator ApiResult(BadRequestResult result)
    {
        return new ApiResult(
            false,
            ApiResultStatusCode.BadRequest);
    }

    public static implicit operator ApiResult(BadRequestObjectResult result)
    {
        return new ApiResult(
            false,
            ApiResultStatusCode.BadRequest,
            result.Value?.ToString());
    }

    public static implicit operator ApiResult(ContentResult result)
    {
        return new ApiResult(
            true,
            ApiResultStatusCode.Success,
            result.Content);
    }

    public static implicit operator ApiResult(NotFoundResult result)
    {
        return new ApiResult(
            false,
            ApiResultStatusCode.NotFound);
    }

    public static implicit operator ApiResult(NotFoundObjectResult result)
    {
        return new ApiResult(
            false,
            ApiResultStatusCode.NotFound,
            result.Value?.ToString());
    }

    #endregion
}

public class ApiResult<TData> : ApiResult
    where TData : class
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public TData? Data { get; set; }

    public ApiResult()
    {
    }

    public ApiResult(
        bool isSuccess,
        ApiResultStatusCode statusCode,
        TData? data,
        string message = "")
        : base(isSuccess, statusCode, message)
    {
        Data = data;
    }

    #region Implicit Operators

    public static implicit operator ApiResult<TData>(TData data)
    {
        return new ApiResult<TData>(
            true,
            ApiResultStatusCode.Success,
            data);
    }

    public static implicit operator ApiResult<TData>(OkResult result)
    {
        return new ApiResult<TData>(
            true,
            ApiResultStatusCode.Success,
            null);
    }

    public static implicit operator ApiResult<TData>(OkObjectResult result)
    {
        return new ApiResult<TData>(
            true,
            ApiResultStatusCode.Success,
            result.Value as TData);
    }

    public static implicit operator ApiResult<TData>(BadRequestResult result)
    {
        return new ApiResult<TData>(
            false,
            ApiResultStatusCode.BadRequest,
            null);
    }

    public static implicit operator ApiResult<TData>(BadRequestObjectResult result)
    {
        var message = result.Value?.ToString();

        return new ApiResult<TData>(
            false,
            ApiResultStatusCode.BadRequest,
            null,
            message);
    }

    public static implicit operator ApiResult<TData>(ContentResult result)
    {
        return new ApiResult<TData>(
            true,
            ApiResultStatusCode.Success,
            null,
            result.Content);
    }

    public static implicit operator ApiResult<TData>(NotFoundResult result)
    {
        return new ApiResult<TData>(
            false,
            ApiResultStatusCode.NotFound,
            null);
    }

    public static implicit operator ApiResult<TData>(NotFoundObjectResult result)
    {
        return new ApiResult<TData>(
            false,
            ApiResultStatusCode.NotFound,
            result.Value as TData);
    }

    #endregion
}