using E_commerce_API.src.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_commerce_API.src.Infrastructure.Configuration
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("order_items");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");
            builder.ComplexProperty(x => x.UnitPrice, p => p.Property(v => v.Value).HasColumnName("unit_price").HasPrecision(18, 2));
            builder.ComplexProperty(x => x.Quantity, p => p.Property(v => v.Value).HasColumnName("quantity"));
            builder.Property(x => x.OrderId).HasColumnName("order_id");
            builder.Property(x => x.ProductId).HasColumnName("product_id");
            builder.HasOne(x => x.Order).WithMany(y => y.OrderItems).HasForeignKey(x => x.OrderId);
            builder.HasOne(x => x.Product).WithMany(y => y.OrderItems).HasForeignKey(x => x.ProductId);
            builder.HasIndex(x => x.ProductId);
            builder.HasIndex(x => x.OrderId);
        }
    }
}
