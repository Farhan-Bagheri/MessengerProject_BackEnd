namespace ShareMicroservice.Dto.Response.Identity.User;

public class GetUserProfileByUserIdentityIdDto
{
    public string UserIdentityId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string UserName { get; set; }
    public string AvatrUrl { get; set; }
}
