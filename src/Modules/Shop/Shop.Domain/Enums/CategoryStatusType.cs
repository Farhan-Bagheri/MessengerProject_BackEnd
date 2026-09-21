using System.ComponentModel.DataAnnotations;

namespace Shop.Domain.Enums;

/// <summary>
/// وضعیت دسته بندی ها
/// </summary>
public enum CategoryStatusType : Byte
{
    [Display(Name = "غیرفعال")] InActive = 1,
    [Display(Name = "فعال")] Active = 2,
    [Display(Name = "بزودی")] ComingSoon = 3
}
