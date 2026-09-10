namespace ShareMicroservice.Application.Common;

public class ServiceResult
{
    public bool IsSuccess { get; set; }

    public object? Data { get; set; }

    public string? Message { get; set; }
    public List<string>? Errorrs { get; set; }

    public static ServiceResult Success(object? data = null, string? message = "عملیات با موفقیت انجام شد.")
    {
        return new ServiceResult
        {
            IsSuccess = true,
            Data = data,
            Message = message
        };
    }

    public static ServiceResult Error(string? message = "عملیات شکست خورد.", List<string>? errors = null)
    {
        return new ServiceResult
        {
            IsSuccess = false,
            Message = message,
            Errorrs = errors
        };
    }
}