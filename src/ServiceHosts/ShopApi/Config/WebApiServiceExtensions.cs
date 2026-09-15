using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Shop.Application;
using Shop.Configuration;
using System.Text;

namespace ShopApi.Config;

public static class WebApiServiceExtensions
{
    public static IServiceCollection AddWebApiServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        #region Url
        var baseUrl = configuration["AppSettings:BaseUrl"];

        var jwtKey = configuration["Jwt:Key"];
        var jwtIssuer = configuration["Jwt:Issuer"];
        var jwtAudience = configuration["Jwt:Audience"];

        var connectionString = configuration.GetConnectionString("ShopDB");
        #endregion

        #region Dependcy Injection


        #endregion

        #region Database

        services.Configure(connectionString!);

        #endregion

        services.AddOpenApi();

        #region MediatR
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(
                typeof(ApplicationAssemblyReference).Assembly);
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
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtIssuer,

                ValidateAudience = true,
                ValidAudience = jwtAudience,

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