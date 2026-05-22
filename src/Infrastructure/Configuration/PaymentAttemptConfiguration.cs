using E_commerce_API.src.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_commerce_API.src.Infrastructure.Configuration
{
    public class PaymentAttemptConfiguration : IEntityTypeConfiguration<PaymentAttempt>
    {
        public void Configure(EntityTypeBuilder<PaymentAttempt> builder)
        {
            builder.HasKey(x => x.PaymentAttemptId);
            builder.HasOne(x => x.Order).WithMany(y => y.PaymentAttempts).HasForeignKey(x => x.OrderId);
            builder.ComplexProperty(x => x.Amount, p => p.Property(v => v.Value).HasColumnName("amount").HasPrecision(18, 2));
            builder.Property(x => x.PaymentDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Property(x => x.Method).HasConversion<string>();
            builder.Property(x => x.Status).HasConversion<string>();
            builder.HasIndex(x => x.OrderId);
        }
    }
}
