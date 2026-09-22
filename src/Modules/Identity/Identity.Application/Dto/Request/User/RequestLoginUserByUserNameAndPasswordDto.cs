using System.ComponentModel;

namespace Identity.Application.Dto.Request.Identity;

public class RequestLoginUserByUserNameAndPasswordDto
{
    [DefaultValue("Farhan")]
    public string UserName { get; set; }

    [DefaultValue("@Farhan_S7")]
    public string Password { get; set; }
}
