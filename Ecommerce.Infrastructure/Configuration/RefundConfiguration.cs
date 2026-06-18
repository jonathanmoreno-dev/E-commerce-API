using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Infrastructure.Configuration
{
    public class RefundConfiguration : IEntityTypeConfiguration<Refund>
    {
        public void Configure(EntityTypeBuilder<Refund> builder)
        {
            builder.ToTable("refunds");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");
            builder.ComplexProperty(x => x.Quantity, p => p.Property(v => v.Value).HasColumnName("quantity"));
            builder.Property(x => x.RefundDate).HasDefaultValueSql("CURRENT_TIMESTAMP").HasColumnName("refund_date");
            builder.Property(x => x.OrderItemId).HasColumnName("order_item_id");
            builder.HasOne(x => x.OrderItem).WithMany(y => y.Refunds).HasForeignKey(x => x.OrderItemId);
            builder.HasIndex(x => x.OrderItemId);
        }
    }
}
