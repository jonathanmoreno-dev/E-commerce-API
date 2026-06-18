using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Infrastructure.Configuration
{
    public class ShippingConfiguration : IEntityTypeConfiguration<Shipping>
    {
        public void Configure(EntityTypeBuilder<Shipping> builder)
        {
            builder.ToTable("shippings");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");
            builder.Property(x => x.OrderId).HasColumnName("order_id");
            builder.HasOne(x => x.Order).WithOne(y => y.Shipping).HasForeignKey<Shipping>(x => x.OrderId);

            builder.ComplexProperty(x => x.ShippingCost, p => p.Property(v => v.Value).HasColumnName("shipping_cost").HasPrecision(18, 2));

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
            builder.Property(x => x.ShippedDate).HasColumnName("shipped_date");
            builder.Property(x => x.DeliveredDate).HasColumnName("delivered_date");
            builder.Property(x => x.TrackingCode).HasMaxLength(100).HasColumnName("tracking_code");
            builder.Property(x => x.Status).HasConversion<string>().HasColumnName("status");
            builder.HasIndex(x => x.OrderId).IsUnique();
        }
    }
}
