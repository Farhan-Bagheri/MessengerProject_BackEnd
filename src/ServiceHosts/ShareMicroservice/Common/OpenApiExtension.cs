using Asp.Versioning.OpenApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;
using Scalar.AspNetCore;

namespace ShareMicroservice.Common;

public static class OpenApiExtension
{
    public static void AddOpenApiConfig(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddEndpointsApiExplorer();
        service.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
        service.AddApiVersioning(setup =>
        {
            setup.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
            setup.AssumeDefaultVersionWhenUnspecified = true;
            setup.ReportApiVersions = true;
        })
            .AddMvc()
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            })
            .AddOpenApi(options =>
            {
                ConfigureDocument(options);
                AddJwtSecurity(options.Document);
                AddSchemaIdDeduplication(options.Document);
            });
    }

    private static void AddSchemaIdDeduplication(OpenApiOptions documentOptions)
    {
        documentOptions.CreateSchemaReferenceId = (jsonTypeInfo) =>
        {
            var type = jsonTypeInfo.Type;
            return type.FullName?.Replace("+", ".") ?? type.Name;
        };
    }

    private static void ConfigureDocument(VersionedOpenApiOptions options)
    {
        options.Document.AddDocumentTransformer((document, context, cancellationToken) =>
        {
            var isV1 = options.Description.ApiVersion.MajorVersion == 1;

            document.Info.Title = isV1 ? "Identity API" : "Identity Api Services";
            document.Info.Description = isV1 ? "Identity API" : "Identity Api Service";

            document.Servers =
            [
                new OpenApiServer { Url = "/", Description = "Direct Access" },
                new OpenApiServer { Url = "/identity", Description = "Gateway API prefix" }
            ];

            return Task.CompletedTask;
        });
    }

    private static void AddJwtSecurity(OpenApiOptions documentOptions)
    {
        documentOptions.AddDocumentTransformer((document, context, cancellationToken) =>
        {
            document.Components ??= new();
            document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
            document.Components.SecuritySchemes[JwtBearerDefaults.AuthenticationScheme] = new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                Description = "توکن خود را وارد کنید Jwt",
                In = ParameterLocation.Header,
            };

            var schemeRef = new OpenApiSecuritySchemeReference(JwtBearerDefaults.AuthenticationScheme, document);

            document.Security ??= [];
            document.Security.Add(new OpenApiSecurityRequirement
            {
                [schemeRef] = []
            });

            return Task.CompletedTask;
        });
    }

    public static void UseCustomScalar(this WebApplication app)
    {
        app.MapOpenApi().WithDocumentPerVersion();

        app.MapScalarApiReference(options =>
        {
            options.Title = "API";
            options.ShowSidebar = true;
            options.HideModels = false;
            options.DefaultOpenAllTags = false;
        });
    }
}