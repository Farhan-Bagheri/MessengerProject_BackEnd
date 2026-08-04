using System.Reflection;
using Microsoft.EntityFrameworkCore;
using ShareMicroservice.Domain.Entities;

namespace Identity.Infrastructure.Configurations;

public static class BaseEntityConfiguration
{
    private static void Configure<TEntity, T>(ModelBuilder modelBuilder)
        where TEntity : BaseEntity
    {
        modelBuilder.Entity<TEntity>(builder =>
        {
            builder.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
        });
    }

    private static void ConfigureBaseEntity<TEntity>(ModelBuilder modelBuilder)
        where TEntity : BaseEntity
    {
        modelBuilder.Entity<TEntity>(builder =>
        {
            builder.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
        });
    }

    public static ModelBuilder ApplyBaseEntityConfiguration(this ModelBuilder modelBuilder)
    {
        var genericMethod = typeof(BaseEntityConfiguration).GetTypeInfo().DeclaredMethods
            .Single(m => m.Name == nameof(Configure));
        var baseMethod = typeof(BaseEntityConfiguration).GetTypeInfo().DeclaredMethods
            .Single(m => m.Name == nameof(ConfigureBaseEntity));
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (entityType.ClrType.IsGenericBaseEntity(out var T))
                genericMethod.MakeGenericMethod(entityType.ClrType, T).Invoke(null, new[] { modelBuilder });
            if (entityType.ClrType.IsBaseEntity())
                baseMethod.MakeGenericMethod(entityType.ClrType).Invoke(null, new[] { modelBuilder });
        }

        return modelBuilder;
    }

    static bool IsGenericBaseEntity(this Type type, out Type T)
    {
        for (var baseType = type.BaseType; baseType != null; baseType = baseType.BaseType)
        {
            if (!baseType.IsGenericType || baseType.GetGenericTypeDefinition() != typeof(BaseEntity)) continue;
            T = baseType.GetGenericArguments()[0];
            return true;
        }

        T = null!;
        return false;
    }

    private static bool IsBaseEntity(this Type type)
    {
        for (var currentType = type; currentType != null; currentType = currentType.BaseType)
        {
            if (currentType == typeof(BaseEntity))
                return true;
        }

        return false;
    }
}