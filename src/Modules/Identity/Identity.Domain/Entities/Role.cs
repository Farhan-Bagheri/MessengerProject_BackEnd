using Microsoft.AspNetCore.Identity;

namespace Identity.Domain.Entities;

public class Role : IdentityRole<Guid>
{
    /// <summary>
    /// Description
    /// </summary>
    public string? Description { get; set; }

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
}