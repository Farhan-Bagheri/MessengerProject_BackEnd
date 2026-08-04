using Identity.Application;
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

        #endregion

        #region Dependcy Injection

        services.AddScoped<IUserFacade, UserFacade>();

        #endregion

        #region DB Context

        services.AddDbContext<IdentityContext>(options =>
        {
            options.UseNpgsql(
                configuration.GetConnectionString("Messenger_Identity"));
        });

        #endregion

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

        #region Swagger
        /*
                services.AddEndpointsApiExplorer();

                services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = "Identity API",
                        Version = "v1"
                    });

                    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        Scheme = "Bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Description = "Enter JWT Token"
                    });

                    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                                {
                                    new OpenApiSecurityScheme
                                    {
                                        Reference = new OpenApiReference
                                        {
                                            Type = ReferenceType.SecurityScheme,
                                            Id = "Bearer"
                                        }
                                    },
                                    Array.Empty<string>()
                                }
                    });
                });
        */
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

                var result = ApiResult.Failure("Validation Error", 400, errors);

                return new BadRequestObjectResult(result);
            };
        });

        #endregion

        return services;
    }
}