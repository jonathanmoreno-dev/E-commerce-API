using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Infrastructure.Configuration
{
    public class CheckoutItemConfiguration : IEntityTypeConfiguration<CheckoutItem>
    {
        public void Configure(EntityTypeBuilder<CheckoutItem> builder)
        {
            builder.ToTable("checkout_items");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");
            builder.Property(x => x.CheckoutId).HasColumnName("checkout_id");
            builder.HasOne(x => x.Checkout).WithMany(y => y.CheckoutItems).HasForeignKey(x => x.CheckoutId);
            builder.Property(x => x.ProductId).HasColumnName("product_id");
            builder.HasOne(x => x.Product).WithMany().HasForeignKey(x => x.ProductId);
            builder.ComplexProperty(x => x.UnitPrice, p => p.Property(v => v.Value).HasColumnName("unit_price").HasPrecision(18, 2));
            builder.ComplexProperty(x => x.Quantity, p => p.Property(v => v.Value).HasColumnName("quantity"));
            builder.HasIndex(x => x.CheckoutId);
            builder.HasIndex(x => x.ProductId);
        }
    }
}
