using Microsoft.AspNetCore.Identity;

namespace Identity.Domain.Entities;

public class User : IdentityUser<Guid>
{
    /// <summary>
    /// FirstName
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// LastName
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// Created Time
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Updated Time
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// IsDelete
    /// </summary>
    public bool IsDelete { get; set; }

    /// <summary>
    /// isActive
    /// </summary>
    public bool IsActive { get; set; }

    #region Relations

    /// <summary>
    /// User Avatars
    /// </summary>
    public IEnumerable<UserAvatar>? UserAvatars { get; set; }

    #endregion
}