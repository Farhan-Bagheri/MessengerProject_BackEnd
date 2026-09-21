using ShareMicroservice.Domain.Entities;
using Shop.Domain.Method;

namespace Shop.Domain.Entities;

/// <summary>
/// فروشگاه
/// </summary>
public class Store : BaseEntity
{
    /// <summary>
    /// شناسه کاربر
    /// </summary>
    public string UserIdentityId { get; set; }

    /// <summary>
    /// نام فروشگاه
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// تلفن تماس
    /// </summary>
    public string PhoneNumber { get; set; }

    /// <summary>
    /// شناسه منحصر به فرد
    /// </summary>
    public string UniqueCode { get; set; }

    /// <summary>
    /// تصویر
    /// </summary>
    public string AvatarUrl { get; set; }

    #region Releation
    public ICollection<StoreProduct> StoreProducts { get; set; } = [];
    #endregion

    #region Method
    public void Create(string name, string phoneNumber, string avatarUrl)
    {
        Name = name;
        PhoneNumber = phoneNumber;
        AvatarUrl = avatarUrl;
        UniqueCode = UniqueCodeGenerator.Generate();
    }

    public void Edit(string name, string phoneNumber, string avatarUrl)
    {
        Name = name;
        PhoneNumber = phoneNumber;
        AvatarUrl = avatarUrl;
    }
    #endregion
}
