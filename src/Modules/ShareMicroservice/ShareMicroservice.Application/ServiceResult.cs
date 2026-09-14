namespace ShareMicroservice.Application;

public class ServiceResult
{
    public bool IsSuccess { get; set; }

    public object? Data { get; set; }

    public string? Message { get; set; }
    public List<string>? Errorrs { get; set; }

    public static ServiceResult Success(object? data = null, string? message = "Success")
    {
        return new ServiceResult
        {
            IsSuccess = true,
            Data = data,
            Message = message
        };
    }

    public static ServiceResult Error(object? data = null, string? message = "Error", List<string>? errors = null)
    {
        return new ServiceResult
        {
            IsSuccess = false,
            Data = data,
            Message = message,
            Errorrs = errors
        };
    }
}