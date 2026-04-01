using E_commerce_API.src.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_commerce_API.src.Infrastructure.Data.Configuration
{
    public class ShippingConfiguration : IEntityTypeConfiguration<Shipping>
    {
        public void Configure(EntityTypeBuilder<Shipping> builder)
        {
            builder.HasKey(x => x.ShippingId);

            builder.HasOne(x => x.Order).WithOne(y => y.Shipping).HasForeignKey<Shipping>(x => x.OrderId);

            builder.Property(x => x.RecipientName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Address).IsRequired().HasMaxLength(200);
            builder.Property(x => x.State).IsRequired().HasMaxLength(50);
            builder.Property(x => x.City).IsRequired().HasMaxLength(100);
            builder.Property(x => x.ZipCode).IsRequired().HasMaxLength(20);

            builder.Property(x => x.ShippingCost).HasPrecision(10, 2);
            builder.Property(x => x.TrackingCode).HasMaxLength(100);
            builder.Property(x => x.Status).HasConversion<string>();
            builder.HasIndex(x => x.OrderId).IsUnique();
        }
    }
}
