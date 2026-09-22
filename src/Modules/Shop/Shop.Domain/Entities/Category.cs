using ShareMicroservice.Domain.Entities;
using Shop.Domain.Enums;

namespace Shop.Domain.Entities;

/// <summary>
/// دسته بندی
/// </summary>
public class Category : BaseEntity
{
    /// <summary>
    /// نام
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// اسلاگ
    /// </summary>
    public string Slug { get; set; }

    /// <summary>
    /// وضعیت
    /// </summary>
    public CategoryStatusType StatusType { get; set; }

    #region Relations
    /// <summary>
    /// دسته بندی والد
    /// </summary>
    public long ParentId { get; set; }
    public Category Parent { get; set; }

    /// <summary>
    /// دسته بندی های فرزند
    /// </summary>
    public ICollection<Category> Childs { get; set; } = [];
    #endregion

    #region Method
    public Category()
    {

    }

    public Category(string name, string slug, CategoryStatusType statusType, long parentId)
    {
        Name = name;
        Slug = slug;
        StatusType = statusType;
        ParentId = parentId;
    }
    #endregion
}
