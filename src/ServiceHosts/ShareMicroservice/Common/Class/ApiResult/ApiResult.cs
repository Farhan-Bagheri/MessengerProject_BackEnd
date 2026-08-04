using Microsoft.AspNetCore.Mvc;

namespace ShareMicroservice.Common.Class.ApiResult;

public class ApiResult<T>
{
    public bool Success { get; set; }
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }
    public List<string> Errors { get; set; }

    public ApiResult(bool success, int statusCode, T data, string message, List<string> errors = null)
    {
        Success = success;
        StatusCode = statusCode;
        Data = data;
        Message = message;
        Errors = errors;
    }

    public ApiResult()
    {
    }

    public static ApiResult<T> SuccessResult(T data, string message = "Successfully", int statusCode = 200)
    {
        return new ApiResult<T>(true, statusCode, data, message);
    }

    public static ApiResult<T> Failure(int statusCode, List<string> errors = null, string message = "Error")
    {
        return new ApiResult<T>(false, statusCode, default, message, errors);
    }
}

public class ApiResult
{
    public bool Success { get; set; }
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public List<string> Errors { get; set; }

    public ApiResult(bool success, int statusCode, string message, List<string> errors = null)
    {
        Success = success;
        StatusCode = statusCode;
        Message = message;
        Errors = errors;
    }

    public static ApiResult SuccessResult(string message = null)
    {
        return new ApiResult(true, 200, message);
    }

    public static ApiResult Failure(string message, int statusCode, List<string> errors = null)
    {
        return new ApiResult(false, statusCode, message, errors);
    }
}
