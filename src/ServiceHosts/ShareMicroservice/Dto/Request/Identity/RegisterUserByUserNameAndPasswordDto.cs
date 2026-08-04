namespace ShareMicroservice.Dto.Request.Identity;

public class RegisterUserByUserNameAndPasswordDto
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}