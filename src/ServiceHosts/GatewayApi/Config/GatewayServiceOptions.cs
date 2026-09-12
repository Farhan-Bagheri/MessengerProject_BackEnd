namespace GatewayApi.Config;

public sealed class GatewayServiceOptions
{
    public string BaseUrl { get; set; } = string.Empty;

    public string Prefix { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;
}