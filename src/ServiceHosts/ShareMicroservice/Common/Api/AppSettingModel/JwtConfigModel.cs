namespace ShareMicroservice.Common.Api.AppSettingModel;

/// <summary>
/// Jwt Config Model on AppSetting Json File
/// </summary>
public class JwtConfigModel
{
    public string Key { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string Expires { get; set; }
    public int AccessTokenExpirationMinutes { get; set; }
    public int RefreshTokenExpirationMinutes { get; set; }
    public bool AllowMultipleLoginsFromTheSameUser { get; set; }
    public bool AllowLogoutAllUserActiveClients { get; set; }
}