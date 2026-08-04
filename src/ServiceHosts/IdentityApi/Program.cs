using IdentityApi.Config;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

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