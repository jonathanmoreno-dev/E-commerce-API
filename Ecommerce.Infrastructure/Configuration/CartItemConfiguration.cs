using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Infrastructure.Configuration
{
    public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.ToTable("cart_items");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");
            builder.ComplexProperty(x => x.UnitPrice, p => p.Property(v => v.Value).HasColumnName("unit_price").HasPrecision(18, 2));
            builder.ComplexProperty(x => x.Quantity, p => p.Property(v => v.Value).HasColumnName("quantity"));
            builder.Property(x => x.CartId).HasColumnName("cart_id");
            builder.Property(x => x.ProductId).HasColumnName("product_id");
            builder.HasOne(x => x.Cart).WithMany(y => y.CartItems).HasForeignKey(x => x.CartId);
            builder.HasOne(x => x.Product).WithMany().HasForeignKey(x => x.ProductId);
            builder.HasIndex(x => x.CartId);
            builder.HasIndex(x => x.ProductId);
        }
    }
}
