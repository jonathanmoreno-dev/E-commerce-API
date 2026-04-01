using E_commerce_API.src.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_commerce_API.src.Infrastructure.Data.Configuration
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(x => x.PaymentId);
            builder.HasOne(x => x.Order).WithMany(y => y.Payments).HasForeignKey(x => x.OrderId);
            builder.Property(x => x.Amount).HasPrecision(18, 2);
            builder.Property(x => x.PaymentDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Property(x => x.Method).HasConversion<string>();
            builder.Property(x => x.Status).HasConversion<string>();
            builder.HasIndex(x => x.OrderId).IsUnique();
        }
    }
}
