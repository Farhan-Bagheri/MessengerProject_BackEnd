using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shop.Domain.Const;
using Shop.Infrastructure.Context;

namespace Shop.Infrastructure;

public class InfrastructureBootstrapper
{
    public static void RegisterDependency(IServiceCollection services, string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
            ArgumentException.ThrowIfNullOrEmpty(nameof(connectionString));

        services.AddDbContext<ShopContext>(option =>
        {
            option.UseSqlServer(
                connectionString,
                x => x.MigrationsHistoryTable("_MigrationsHistory", ShopSchema.Shop));
        });

        services.AddScoped<IShopContext, ShopContext>();
    }
}
