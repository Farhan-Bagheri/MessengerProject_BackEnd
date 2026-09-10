namespace ShareMicroservice.Domain.Entities;

public class BaseEntity
{
    /// <summary>
    /// Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// CreatedAt
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// UpdatedAt
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// IsDelete
    /// </summary>
    public bool IsDelete { get; set; } = false;
}