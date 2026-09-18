using ShareMicroservice.Domain.Class;
using ShareMicroservice.Domain.Entities;

namespace Shop.Domain.Entities;

/// <summary>
/// محصول
/// </summary>
public class Product : BaseEntity
{
    /// <summary>
    /// تایتل
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// توضیحات
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// تصاویر
    /// </summary>
    public List<ImageUrl> Images { get; set; } = [];

    #region Releation
    public IEnumerable<StoreProduct> StoreProducts { get; set; }
    #endregion
}
