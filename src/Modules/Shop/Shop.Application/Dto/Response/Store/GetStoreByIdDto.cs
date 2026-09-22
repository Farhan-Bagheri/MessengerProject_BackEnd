namespace Shop.Application.Dto.Response.Store;

public class GetStoreByIdDto
{
    public string UserIdentityId { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string UniqueCode { get; set; }
    public string AvatarUrl { get; set; }
    public string CreatedAt { get; set; }
    public string UpdatedAt { get; set; }
}
