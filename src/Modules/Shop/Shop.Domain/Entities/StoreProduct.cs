using ShareMicroservice.Domain.Entities;

namespace Shop.Domain.Entities;

/// <summary>
/// واسط فروشگاه و کالا
/// </summary>
public class StoreProduct : BaseEntity
{
    /// <summary>
    /// قیمت
    /// </summary>
    public long Price { get; set; }

    /// <summary>
    /// موجودی
    /// </summary>
    public int StockQuantity { get; set; }

    /// <summary>
    /// در دسترس است؟
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// اسلاگ
    /// </summary>
    public string Slug { get; set; }

    #region Releation
    /// <summary>
    /// محصول
    /// </summary>
    public long ProductId { get; set; }
    public Product Product { get; set; }

    /// <summary>
    /// فروشگاه
    /// </summary>
    public long StoreId { get; set; }
    public Store Store { get; set; }
    #endregion
}
