using Identity.Application;
using Identity.Configuration;
using Identity.Domain.Entities;
using Identity.Facade.User;
using Identity.Infrastructure.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShareMicroservice.Common.Class.ApiResult;
using System.Text;


namespace IdentityApi.Config;

public static class WebApiServiceExtensions
{
    public static IServiceCollection AddWebApiServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        #region Url
        var baseUrl = configuration["AppSettings:BaseUrl"];
        var jwtKey = configuration["Jwt:Key"];
        var connectionString = configuration.GetConnectionString("IdentityDB");
        #endregion

        #region Dependcy Injection

        services.AddScoped<IUserFacade, UserFacade>();

        #endregion

        services.Configure(connectionString!);

        services.AddOpenApi();

        services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<IdentityContext>()
                .AddDefaultTokenProviders();

        #region MediatR

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(
                typeof(ApplicationAssemblyReference).Assembly);
        });

        #endregion

        #region API Versioning

        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;

            options.ApiVersionReader = new UrlSegmentApiVersionReader();
        });

        services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        #endregion

        #region JWT

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = baseUrl,
                    ValidAudience = baseUrl,

                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtKey!))
                };
            });

        #endregion

        #region Api Validation Error

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState
                    .Where(x => x.Value!.Errors.Count > 0)
                    .SelectMany(x => x.Value!.Errors)
                    .Select(x => x.ErrorMessage)
                    .ToList();

                var result = ApiResult.Failure("Validation Error", errors);

                return new BadRequestObjectResult(result);
            };
        });

        #endregion

        return services;
    }
}