using E_commerce_API.src.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_commerce_API.src.Infrastructure.Configuration
{
    public class ShippingConfiguration : IEntityTypeConfiguration<Shipping>
    {
        public void Configure(EntityTypeBuilder<Shipping> builder)
        {
            builder.HasKey(x => x.ShippingId);

            builder.HasOne(x => x.Order).WithOne(y => y.Shipping).HasForeignKey<Shipping>(x => x.OrderId);

            builder.ComplexProperty(x => x.ShippingCost, p => p.Property(v => v.Value).HasColumnName("ShippingCost").HasPrecision(18, 2));

            builder.ComplexProperty(x => x.ShippingAddress, p =>
            {
                p.Property(r => r.RecipientName).HasColumnName("RecipientName").IsRequired().HasMaxLength(100);
                p.Property(r => r.PhoneNumber).HasColumnName("PhoneNumber").IsRequired().HasMaxLength(20);
                p.Property(r => r.Street).HasColumnName("Neighborhood").IsRequired().HasMaxLength(100);
                p.Property(r => r.Number).HasColumnName("Number").IsRequired().HasMaxLength(50);
                p.Property(r => r.State).HasColumnName("State").IsRequired().HasMaxLength(20);
                p.Property(r => r.City).HasColumnName("City").IsRequired().HasMaxLength(50);
                p.Property(r => r.ZipCode).HasColumnName("ZipCode").IsRequired().HasMaxLength(20);
            });

            builder.Property(x => x.TrackingCode).HasMaxLength(100);
            builder.Property(x => x.Status).HasConversion<string>();
            builder.HasIndex(x => x.OrderId).IsUnique();
        }
    }
}
