using GatewayApi.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var env = builder.Environment.EnvironmentName;
Console.ForegroundColor = ConsoleColor.Red;
Console.WriteLine($"Application Running With Name : {builder.Environment.ApplicationName} ----> Environment : {env}");
Console.WriteLine();

#region Jwt

var jwtSettings = builder.Configuration.GetSection("Jwt");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidIssuer = jwtSettings["Issuer"],

            ValidateAudience = false,
            ValidAudience = jwtSettings["Audience"],

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!)),

            ValidateLifetime = true
        };
    });


#endregion

builder.Services.AddAuthorization();

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddControllers();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseCustomeScalarApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapReverseProxy();

app.MapControllers();

app.Run();
