using ShareMicroservice.Domain.Entities;

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
    /// آدرس
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// تصویر
    /// </summary>
    public string ImageUrl { get; set; }

    #region Releation
    public IEnumerable<StoreProduct> StoreProducts { get; set; }
    #endregion
}
