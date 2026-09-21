using ShareMicroservice.Domain.Entities;
using Shop.Domain.Extension;

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
    /// اسلاگ
    /// </summary>
    public string Slug { get; set; }

    /// <summary>
    /// در دسترس است؟
    /// </summary>
    public bool IsActive { get; set; }

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

    #region Method
    public void Create(long price, int stockQuantity, bool isActive, long productId, long storeId)
    {
        Price = price;
        StockQuantity = stockQuantity;
        IsActive = isActive;
        ProductId = productId;
        StoreId = storeId;
        Slug = $"{Product.Title.ToSlug()}-{Product.UniqueCode}-{Store.UniqueCode}";
    }

    public void Edit(long price, int stockQuantity, bool isActive)
    {
        Price = price;
        StockQuantity = stockQuantity;
        IsActive = isActive;
    }
    #endregion
}
