using GatewayApi.Config;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace GatewayApi.Controllers;

[ApiController]
public sealed class GatewayOpenApiController(
    IConfiguration configuration,
    IHttpClientFactory httpClientFactory) : ControllerBase
{
    [HttpGet("/openapi/{module}/v1.json")]
    public async Task<IActionResult> Get(
        string module,
        CancellationToken cancellationToken)
    {
        var service = configuration
            .GetSection($"Services:{module}")
            .Get<GatewayServiceOptions>();

        if (service is null)
        {
            return NotFound(
                $"Service '{module}' is not configured.");
        }

        if (string.IsNullOrWhiteSpace(service.BaseUrl))
        {
            return Problem(
                $"BaseUrl for service '{module}' is not configured.");
        }

        var openApiUrl =
            $"{service.BaseUrl.TrimEnd('/')}/openapi/v1.json";

        var client = httpClientFactory.CreateClient();

        using var response = await client.GetAsync(
            openApiUrl,
            cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            return StatusCode(
                (int)response.StatusCode,
                $"Unable to load OpenAPI document for '{module}'.");
        }

        var json = await response.Content.ReadAsStringAsync(
            cancellationToken);

        using var document = JsonDocument.Parse(json);

        await using var stream = new MemoryStream();

        await using var writer = new Utf8JsonWriter(
            stream,
            new JsonWriterOptions
            {
                Indented = true
            });

        writer.WriteStartObject();

        foreach (var property in document.RootElement.EnumerateObject())
        {
            if (property.NameEquals("servers"))
                continue;

            property.WriteTo(writer);
        }

        writer.WritePropertyName("servers");

        writer.WriteStartArray();

        writer.WriteStartObject();

        writer.WriteString(
            "url",
            service.Prefix);

        writer.WriteEndObject();

        writer.WriteEndArray();

        writer.WriteEndObject();

        await writer.FlushAsync(cancellationToken);

        return File(
            stream.ToArray(),
            "application/json");
    }
}