using ShareMicroservice.Domain.Entities;

namespace Shop.Domain.Entities;

/// <summary>
/// فروشگاه
/// </summary>
public class Store : BaseEntity
{
    /// <summary>
    /// آیدی نماینده
    /// </summary>
    public long UserId { get; set; }

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

    #region Relations
    public User User { get; set; }
    #endregion
}
