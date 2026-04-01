using E_commerce_API.src.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_commerce_API.src.Infrastructure.Data.Configuration
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(x => x.OrderItemId);
            builder.Property(x => x.UnitPrice).HasPrecision(18, 2);
            builder.HasOne(x => x.Order).WithMany(y => y.OrderItems).HasForeignKey(x => x.OrderId);
            builder.HasOne(x => x.Product).WithMany(y => y.OrderItems).HasForeignKey(x => x.ProductId);
            builder.HasIndex(x => x.ProductId).IsUnique();
            builder.HasIndex(x => x.OrderId).IsUnique();
        }
    }
}
