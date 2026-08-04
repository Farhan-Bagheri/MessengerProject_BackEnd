using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Identity.Application;
using Identity.Domain.Entities;
using Identity.Facade.User;
using Identity.Infrastructure.Context;
using IdentityApi.Config;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShareMicroservice.Common.Class.ApiResult;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddWebApiServices(builder.Configuration);

builder.Services
    .AddIdentity<User, Role>()
    .AddEntityFrameworkStores<IdentityContext>()
    .AddDefaultTokenProviders();

var app = builder.Build();


app.UseHttpsRedirection();

app.UseRouting();

app.UseGlobalException();

app.MapControllers();

app.Run();