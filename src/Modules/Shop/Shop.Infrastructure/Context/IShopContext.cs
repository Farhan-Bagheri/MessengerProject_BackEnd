using Microsoft.EntityFrameworkCore;
using ShareMicroservice.Domain.Entities;
using Shop.Domain.Const;
using Shop.Domain.Entities;
using Shop.Infrastructure.Configurations;
using System.Linq.Expressions;

namespace Shop.Infrastructure.Context;

public interface IShopContext
{

    public DbSet<Product> Products { get; set; }
    public DbSet<Store> Stores { get; set; }
    public DbSet<StoreProduct> StoreProducts { get; set; }
    public DbSet<Category> Categories { get; set; }

    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
public class ShopContext(DbContextOptions<ShopContext> options) : DbContext(options), IShopContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Store> Stores { get; set; }
    public DbSet<StoreProduct> StoreProducts { get; set; }
    public DbSet<Category> Categories { get; set; }


    #region OnModelCreating
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(ShopSchema.Shop);
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new ProductConfig());
        modelBuilder.ApplyConfiguration(new StoreConfig());
        modelBuilder.ApplyConfiguration(new StoreProductConfig());
        modelBuilder.ApplyConfiguration(new CategoryConfig());

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                entityType.SetQueryFilter(CreateIsDeleteFilter(entityType.ClrType));
            }
        }
    }

    private static LambdaExpression CreateIsDeleteFilter(Type entityType)
    {
        var parameter = Expression.Parameter(entityType, "e");

        var property = Expression.Property(
            parameter,
            nameof(BaseEntity.IsDelete));

        var body = Expression.Equal(
            property,
            Expression.Constant(false));

        return Expression.Lambda(body, parameter);
    }
    #endregion

    #region SaveChange
    public override int SaveChanges()
    {
        SetSoftDelete();
        SetAuditProperties();

        try
        {
            return base.SaveChanges();
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new DbUpdateConcurrencyException("A concurrency conflict occurred while saving changes.");
        }
    }

    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        SetSoftDelete();
        SetAuditProperties();

        try
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new DbUpdateConcurrencyException("A concurrency conflict occurred while saving changes.");
        }
    }

    private void SetSoftDelete()
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Modified;
                entry.Entity.IsDelete = true;
            }
        }
    }

    private void SetAuditProperties()
    {
        var now = DateTime.Now;

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = now;
                    entry.Entity.UpdatedAt = now;
                    break;

                case EntityState.Modified:
                    entry.Property(x => x.CreatedAt).IsModified = false;
                    entry.Entity.UpdatedAt = now;
                    break;
            }
        }
    }
    #endregion
}