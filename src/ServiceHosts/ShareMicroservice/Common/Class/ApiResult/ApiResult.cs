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

    public static ApiResult<T> SuccessResult(T data, string message = "Successfully")
    {
        return new ApiResult<T>(true, 200, data, message);
    }

    public static ApiResult<T> Failure(string message = "Error", List<string> errors = null)
    {
        return new ApiResult<T>(false, 400, default, message, errors);
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

    public static ApiResult Failure(string message, List<string> errors = null)
    {
        return new ApiResult(false, 400, message, errors);
    }
}
