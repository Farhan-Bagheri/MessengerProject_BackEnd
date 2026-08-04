using ShareMicroservice.Domain.Entities;

namespace Identity.Domain.Entities;

public class UserAvatar : BaseEntity
{
    /// <summary>
    /// User
    /// </summary>
    public Guid UserId { get; set; }

    public User User { get; set; }

    /// <summary>
    /// Url
    /// </summary>
    public string Url { get; set; }
}