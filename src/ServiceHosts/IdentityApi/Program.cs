using IdentityApi.Config;
using ShareMicroservice.Common;

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

builder.Services.AddOpenApiConfig(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddWebApiServices(builder.Configuration);

var app = builder.Build();


app.UseHttpsRedirection();

app.UseRouting();

app.UseGlobalException();

app.UseCustomScalar();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();