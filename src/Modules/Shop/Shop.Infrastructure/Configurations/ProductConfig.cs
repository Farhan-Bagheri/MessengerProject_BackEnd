using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shop.Domain.Entities;

namespace Shop.Infrastructure.Configurations;

public class ProductConfig : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.UniqueCode).IsUnique();

        builder.OwnsMany(x => x.Images, o =>
        {
            o.ToJson();
            o.Property(x => x.Url).IsRequired();
        });
    }
}
