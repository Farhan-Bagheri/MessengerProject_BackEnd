using GatewayApi.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var env = builder.Environment.EnvironmentName;
//var env = "Production";

Console.ForegroundColor = ConsoleColor.Red;
Console.WriteLine(
    $"Application Running With Name : " +
    $"{builder.Environment.ApplicationName} ----> Environment : {env}");
Console.ResetColor();

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

var jwtSettings = builder.Configuration.GetSection("Jwt");

var jwtKey = jwtSettings["Key"];

if (string.IsNullOrWhiteSpace(jwtKey))
{
    throw new InvalidOperationException(
        "JWT Key is not configured.");
}

builder.Services
    .AddAuthentication(
        JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtSettings["Issuer"],

                ValidateAudience = true,
                ValidAudience = jwtSettings["Audience"],

                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtKey!))
            };
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.ForegroundColor = ConsoleColor.Red;

                Console.WriteLine("========== JWT AUTH FAILED ==========");
                Console.WriteLine(context.Exception.Message);
                Console.WriteLine(context.Exception);
                Console.WriteLine("=====================================");

                Console.ResetColor();

                return Task.CompletedTask;
            },

            OnTokenValidated = context =>
            {
                Console.ForegroundColor = ConsoleColor.Green;

                Console.WriteLine("========== JWT VALIDATED ==========");
                Console.WriteLine($"User: {context.Principal?.Identity?.Name}");
                Console.WriteLine("===================================");

                Console.ResetColor();

                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddHttpClient();

builder.Services
    .AddReverseProxy()
    .LoadFromConfig(
        builder.Configuration.GetSection(
            "ReverseProxy"));

var app = builder.Build();

app.UseCustomScalarApi(builder.Configuration);

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapReverseProxy();


app.Run();