using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shop.Domain.Entities;

namespace Shop.Infrastructure.Configurations;

public class StoreProductConfig : IEntityTypeConfiguration<StoreProduct>
{
    public void Configure(EntityTypeBuilder<StoreProduct> builder)
    {
        builder.ToTable("StoreProducts");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Slug).IsUnicode();
        builder.HasIndex(x => x.Slug);

        builder.HasOne(x => x.Product)
            .WithMany(x => x.StoreProducts)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Store)
            .WithMany(x => x.StoreProducts)
            .HasForeignKey(x => x.StoreId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
