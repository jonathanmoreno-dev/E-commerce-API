using E_commerce_API.src.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_commerce_API.src.Infrastructure.Configuration
{
    public class RefundConfiguration : IEntityTypeConfiguration<Refund>
    {
        public void Configure(EntityTypeBuilder<Refund> builder)
        {
            builder.HasKey(x => x.RefundId);
            builder.ComplexProperty(x => x.Quantity, p => p.Property(v => v.Value).HasColumnName("Quantity"));
            builder.Property(x => x.RefundDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.HasOne(x => x.OrderItem).WithMany(y => y.Refunds).HasForeignKey(x => x.OrderItemId);
            builder.HasIndex(x => x.OrderItemId).IsUnique();
        }
    }
}
