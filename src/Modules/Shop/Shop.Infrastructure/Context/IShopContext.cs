using Microsoft.EntityFrameworkCore;
using ShareMicroservice.Domain.Entities;
using Shop.Domain.Const;
using Shop.Domain.Entities;
using Shop.Infrastructure.Configurations;

namespace Shop.Infrastructure.Context;

public interface IShopContext
{

    public DbSet<Product> Products { get; set; }
    public DbSet<Store> Stores { get; set; }
}
public class ShopContext(DbContextOptions<ShopContext> options) : DbContext(options), IShopContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Store> Stores { get; set; }

    #region SaveChange
    public override int SaveChanges()
    {
        SetAuditProperties();

        try
        {
            return base.SaveChanges();
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new DbUpdateConcurrencyException(
                "A concurrency conflict occurred while saving changes.");
        }
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetAuditProperties();

        try
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new DbUpdateConcurrencyException(
                "A concurrency conflict occurred while saving changes.");
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

    #region OnModelCreating
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(ShopSchema.Shop);
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new ProductConfig());
        modelBuilder.ApplyConfiguration(new StoreConfig());
    }
    #endregion
}