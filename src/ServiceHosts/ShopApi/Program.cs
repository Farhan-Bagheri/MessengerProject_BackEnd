using Scalar.AspNetCore;
using ShopApi.Config;

var builder = WebApplication.CreateBuilder(args);

var env = builder.Environment.EnvironmentName;
//var env = "Production";

Console.ForegroundColor = ConsoleColor.Red;
Console.WriteLine(
    $"Application Running With Name : " +
    $"{builder.Environment.ApplicationName} ----> Environment : {env}");
Console.ResetColor();

builder.Services.AddControllers();

builder.Services.AddWebApiServices(builder.Configuration);

var app = builder.Build();

app.UseHttpsRedirection();

app.UseRouting();

app.UseGlobalException();

app.MapOpenApi();
app.MapScalarApiReference();

app.MapControllers();

app.Run();