using E_commerce_API.src.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_commerce_API.src.Infrastructure.Data.Configuration
{
    public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.HasKey(x => x.CartItemId);
            builder.Property(x => x.UnitPrice).HasPrecision(18, 2);
            builder.HasOne(x => x.Cart).WithMany(y => y.CartItems).HasForeignKey(x => x.CartId);
            builder.HasOne(x => x.Product).WithMany(y => y.CartItems).HasForeignKey(x => x.ProductId);
            builder.HasIndex(x => x.CartId).IsUnique();
            builder.HasIndex(x => x.ProductId).IsUnique();
        }
    }
}
