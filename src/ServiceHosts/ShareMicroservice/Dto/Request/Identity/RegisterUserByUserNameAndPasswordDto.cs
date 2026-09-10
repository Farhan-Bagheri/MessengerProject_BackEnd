using System.ComponentModel;

namespace ShareMicroservice.Dto.Request.Identity;

public class RegisterUserByUserNameAndPasswordDto
{
    [DefaultValue("Farhan")]
    public string UserName { get; set; }

    [DefaultValue("@Farhan_S7")]
    public string Password { get; set; }

    [DefaultValue("@Farhan_S7")]
    public string ConfirmPassword { get; set; }
}