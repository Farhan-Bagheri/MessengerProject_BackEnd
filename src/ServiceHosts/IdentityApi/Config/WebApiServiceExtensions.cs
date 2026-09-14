using Identity.Application;
using Identity.Configuration;
using Identity.Domain.Entities;
using Identity.Facade.User;
using Identity.Infrastructure.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;

namespace IdentityApi.Config;

public static class WebApiServiceExtensions
{
    public static IServiceCollection AddWebApiServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        #region Configuration
        var baseUrl = configuration["AppSettings:BaseUrl"];
        var jwtKey = configuration["Jwt:Key"];
        var connectionString = configuration.GetConnectionString("IdentityDB");
        #endregion

        #region Dependency Injection

        services.AddScoped<IUserFacade, UserFacade>();

        #endregion

        #region Database

        services.Configure(connectionString!);

        #endregion

        #region Identity

        services.AddIdentity<User, Role>()
            .AddEntityFrameworkStores<IdentityContext>()
            .AddDefaultTokenProviders();

        #endregion

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

            options.ApiVersionReader =
                new UrlSegmentApiVersionReader();
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
            options.DefaultAuthenticateScheme =
                JwtBearerDefaults.AuthenticationScheme;

            options.DefaultChallengeScheme =
                JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters =
                new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = baseUrl,
                    ValidAudience = baseUrl,

                    IssuerSigningKey =
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtKey!))
                };
        });

        services.AddAuthorization();

        #endregion

        #region OpenAPI

        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer(
                (document, context, cancellationToken) =>
                {
                    document.Components ??= new OpenApiComponents();

                    document.Components.SecuritySchemes =
                        new Dictionary<string, IOpenApiSecurityScheme>
                        {
                            ["Bearer"] =
                                new OpenApiSecurityScheme
                                {
                                    Type = SecuritySchemeType.Http,
                                    Scheme = "bearer",
                                    BearerFormat = "JWT",
                                    Name = "Authorization",
                                    In = ParameterLocation.Header,
                                    Description =
                                        "Enter your JWT Bearer token."
                                }
                        };

                    return Task.CompletedTask;
                });
        });

        #endregion

        #region API Validation Error

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .SelectMany(x => x.Value!.Errors)
                    .Select(x => x.ErrorMessage)
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .ToList();

                return new BadRequestObjectResult(new
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Validation Error",
                    Errors = errors
                });
            };
        });

        #endregion

        return services;
    }
}