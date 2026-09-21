using ShareMicroservice.Domain.Entities;
using Shop.Domain.Class;
using Shop.Domain.Method;

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
    /// شناسه منحصر به فرد
    /// </summary>
    public string UniqueCode { get; set; }

    /// <summary>
    /// تصاویر
    /// </summary>
    public List<ProductImageUrl> Images { get; set; } = [];

    #region Releation
    public ICollection<StoreProduct> StoreProducts { get; set; } = [];
    #endregion

    #region Method
    public void Create(string title, string description, List<ProductImageUrl> images)
    {
        Title = title;
        Description = description;
        Images = images;
        UniqueCode = UniqueCodeGenerator.Generate();
    }

    public void Edit(string title, string description, List<ProductImageUrl> images)
    {
        Title = title;
        Description = description;
        Images = images;
    }
    #endregion
}
