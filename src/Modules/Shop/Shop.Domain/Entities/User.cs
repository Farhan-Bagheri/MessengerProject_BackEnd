using ShareMicroservice.Domain.Entities;

namespace Shop.Domain.Entities;

/// <summary>
/// کاربر
/// </summary>
public class User : BaseEntity
{
    /// <summary>
    /// شناسه کاربر
    /// </summary>
    public string UserIdentityId { get; set; }

    /// <summary>
    /// مسدود است یا خیر؟
    /// </summary>
    public bool IsBanned { get; set; }

    #region Relations
    public ICollection<Store> Shops { get; set; }
    #endregion
}
