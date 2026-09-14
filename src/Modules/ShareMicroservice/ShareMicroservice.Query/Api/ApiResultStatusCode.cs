using System.ComponentModel.DataAnnotations;

namespace ShareMicroservice.Query.Api;

public enum ApiResultStatusCode
{
    [Display(Name = "عملیات با موفقیت انجام شد")]
    Success = 200,

    [Display(Name = "لطفا دقایقی دیگر مراجعه فرمائید.\r\nاختلالی در ارتباط شما با اینترنت به وجود آمده است")]
    ServerError = 500,

    [Display(Name = "اختلالی موقتی در انجام درخواست رخ داده است.\r\nلطفاً کمی بعد دوباره تلاش کنید")]
    BadRequest = 400,

    [Display(Name = "یافت نشد")] NotFound = 404,

    [Display(Name = "لیست خالی است")] ListEmpty = 204,

    [Display(Name = "خطایی در پردازش رخ داد")]
    LogicError = 409,

    [Display(Name = "متأسفیم؛ به نظر می‌رسد که تعداد درخواست‌های شما بیش از حد مجاز است.\r\nلطفا چند لحظه دیگر، مجددا تلاش کنید...")]
    TooManyRequests = 429,

    [Display(Name = "خطای احراز هویت")] UnAuthorized = 401
}