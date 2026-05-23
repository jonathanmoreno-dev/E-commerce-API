using E_commerce_API.src.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_commerce_API.src.Infrastructure.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ComplexProperty(x => x.Name, p => p.Property(v => v.Value).HasColumnName("name").IsRequired().HasMaxLength(255));
            builder.ComplexProperty(x => x.ShortDescription, p => p.Property(v => v.Value).HasColumnName("short_description").IsRequired().HasMaxLength(400));
            builder.ComplexProperty(x => x.LongDescription, p => p.Property(v => v.Value).HasColumnName("long_description").IsRequired());
            builder.ComplexProperty(x => x.Price, p => p.Property(v => v.Value).HasColumnName("price").HasPrecision(18, 2));
            builder.ComplexProperty(x => x.Stock, p => p.Property(v => v.Value).HasColumnName("stock"));
            builder.HasMany(x => x.Categories).WithMany(y => y.Products);
        }
    }
}
