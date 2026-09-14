namespace ShareMicroservice.Query.Api.Jwt;

public class AccessTokenDto
{
    public string AccessToken { get; init; }
    public string RefreshToken { get; init; }
    public string RefreshTokenSerial { get; init; }
    public DateTime ExpireDate { get; set; }
}