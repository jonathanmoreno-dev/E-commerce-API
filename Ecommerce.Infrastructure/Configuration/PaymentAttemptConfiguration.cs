using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Infrastructure.Configuration
{
    public class PaymentAttemptConfiguration : IEntityTypeConfiguration<PaymentAttempt>
    {
        public void Configure(EntityTypeBuilder<PaymentAttempt> builder)
        {
            builder.ToTable("payment_attempts");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");
            builder.Property(x => x.CheckoutId).HasColumnName("checkout_id");
            builder.HasOne(x => x.Checkout).WithMany(y => y.PaymentAttempts).HasForeignKey(x => x.CheckoutId);
            builder.ComplexProperty(x => x.Amount, p => p.Property(v => v.Value).HasColumnName("amount").HasPrecision(18, 2));
            builder.Property(x => x.PaymentDate).HasDefaultValueSql("CURRENT_TIMESTAMP").HasColumnName("payment_date");
            builder.Property(x => x.Method).HasConversion<string>().HasColumnName("method");
            builder.Property(x => x.Status).HasConversion<string>().HasColumnName("status");
            builder.HasIndex(x => x.CheckoutId);
        }
    }
}
