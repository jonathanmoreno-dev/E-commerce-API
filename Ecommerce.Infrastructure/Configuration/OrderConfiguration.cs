using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Infrastructure.Configuration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("orders");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");
            builder.ComplexProperty(x => x.ShippingAddress, p =>
            {
                p.ComplexProperty(r => r.RecipientName, pn => pn.Property(v => v.Value).HasColumnName("recipient_name").IsRequired().HasMaxLength(100));
                p.ComplexProperty(r => r.PhoneNumber, pn => pn.Property(v => v.Value).HasColumnName("phone_number").IsRequired().HasMaxLength(20));
                p.Property(r => r.Neighborhood).HasColumnName("neighborhood").IsRequired().HasMaxLength(100);
                p.Property(r => r.Street).HasColumnName("street").IsRequired().HasMaxLength(50);
                p.Property(r => r.Number).HasColumnName("number").IsRequired().HasMaxLength(50);
                p.Property(r => r.State).HasColumnName("state").IsRequired().HasMaxLength(20);
                p.Property(r => r.City).HasColumnName("city").IsRequired().HasMaxLength(50);
                p.Property(r => r.ZipCode).HasColumnName("zip_code").IsRequired().HasMaxLength(20);
            });
            builder.ComplexProperty(x => x.ShippingCost, p => p.Property(v => v.Value).HasColumnName("shipping_cost").HasPrecision(18, 2));
            builder.Property(x => x.PaymentMethod).HasConversion<string>().HasColumnName("payment_method");
            builder.Ignore(x => x.SubTotal);
            builder.ComplexProperty(x => x.TotalPaid, p => p.Property(v => v.Value).HasColumnName("total_paid").HasPrecision(18,2));
            builder.Property(x => x.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP").HasColumnName("created_at");
            builder.Property(x => x.Status).HasConversion<string>().HasColumnName("status");
            builder.Property(x => x.UserId).HasColumnName("user_id");
            builder.HasOne(x => x.User).WithMany(y => y.Orders).HasForeignKey(x => x.UserId);
            builder.HasIndex(x => x.UserId);
        }
    }
}
