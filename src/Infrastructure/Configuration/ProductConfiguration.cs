using E_commerce_API.src.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_commerce_API.src.Infrastructure.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.ProductId);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(255);
            builder.Property(x => x.ShortDescription).IsRequired().HasMaxLength(400);
            builder.Property(x => x.LongDescription).IsRequired();
            builder.Property(x => x.Price).HasPrecision(18, 2);
            builder.HasMany(x => x.Categories).WithMany(y => y.Products);
        }
    }
}
