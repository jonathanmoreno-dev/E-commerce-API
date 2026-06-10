using E_commerce_API.src.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_commerce_API.src.Infrastructure.Configuration
{
    public class CheckoutConfiguration : IEntityTypeConfiguration<Checkout>
    {
        public void Configure(EntityTypeBuilder<Checkout> builder)
        {
            builder.ToTable("checkouts");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");
            builder.Property(x => x.UserId).HasColumnName("user_id");
            builder.HasOne(x => x.User).WithMany(y => y.Checkouts).HasForeignKey(x => x.UserId);
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
            builder.Ignore(x => x.Total);
            builder.Ignore(x => x.ExpiresAt);
            builder.Property(x => x.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP").HasColumnName("created_at");
            builder.Property(x => x.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP").HasColumnName("updated_at");
            builder.HasIndex(x => x.UserId).IsUnique();
        }
    }
}
