using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Infrastructure.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("products");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");
            builder.ComplexProperty(x => x.Name, p => p.Property(v => v.Value).HasColumnName("name").IsRequired().HasMaxLength(255));
            builder.ComplexProperty(x => x.ShortDescription, p => p.Property(v => v.Value).HasColumnName("short_description").IsRequired().HasMaxLength(400));
            builder.ComplexProperty(x => x.LongDescription, p => p.Property(v => v.Value).HasColumnName("long_description").IsRequired());
            builder.ComplexProperty(x => x.Price, p => p.Property(v => v.Value).HasColumnName("price").HasPrecision(18, 2));
            builder.ComplexProperty(x => x.Stock, p => p.Property(v => v.Value).HasColumnName("stock"));
            builder.OwnsMany(x => x.ProductImages, p =>
            {
                p.Property(v => v.Url).HasColumnName("product_image_url").IsRequired();
                p.Property(v => v.Order).HasColumnName("product_image_order");
            });

            builder.HasMany(x => x.Categories).WithMany(y => y.Products);
        }
    }
}
