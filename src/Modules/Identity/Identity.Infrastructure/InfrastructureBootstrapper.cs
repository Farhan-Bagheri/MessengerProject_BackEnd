using Identity.Domain.Const;
using Identity.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Infrastructure;

public static class InfrastructureBootstrapper
{
    public static void RegisterDependency(IServiceCollection services, string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
            ArgumentException.ThrowIfNullOrEmpty(nameof(connectionString));

        services.AddDbContext<IdentityContext>(option =>
        {
            option.UseSqlServer(
                connectionString,
                x => x.MigrationsHistoryTable("_MigrationsHistory", IdentitySchema.Identity));
        });

        services.AddScoped<IIdentityContext, IdentityContext>();
    }
}
