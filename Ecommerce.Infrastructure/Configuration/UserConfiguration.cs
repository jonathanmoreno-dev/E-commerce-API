using Ecommerce.Domain.Entities;
using Ecommerce.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Infrastructure.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");
            builder.HasIndex(x => x.Email).IsUnique();

            builder.ComplexProperty(x => x.FullName, p => p.Property(v => v.Value).HasColumnName("full_name").IsRequired().HasMaxLength(150));
            builder.Property(x => x.Email).HasConversion(v => v.Value, v => new Email(v)).HasColumnName("email").IsRequired().HasMaxLength(255);
            builder.ComplexProperty(x => x.PhoneNumber, p => p.Property(v => v.Value).HasColumnName("phone_number").IsRequired().HasMaxLength(50));
            builder.OwnsOne(x => x.AvatarImage, p =>
            {
                p.Property(v => v!.Url)
                 .HasColumnName("avatar_image_url")
                 .IsRequired(false);
            });
            builder.OwnsMany(x => x.ShippingAddresses, p =>
            {
                p.OwnsOne(r => r.RecipientName, pn => pn.Property(v => v.Value).HasColumnName("recipient_name").IsRequired().HasMaxLength(100));
                p.OwnsOne(r => r.PhoneNumber, pn => pn.Property(v => v.Value).HasColumnName("phone_number").IsRequired().HasMaxLength(20));
                p.Property(r => r.Neighborhood).HasColumnName("neighborhood").IsRequired().HasMaxLength(100);
                p.Property(r => r.Street).HasColumnName("street").IsRequired().HasMaxLength(50);
                p.Property(r => r.Number).HasColumnName("number").IsRequired().HasMaxLength(50);
                p.Property(r => r.State).HasColumnName("state").IsRequired().HasMaxLength(20);
                p.Property(r => r.City).HasColumnName("city").IsRequired().HasMaxLength(50);
                p.Property(r => r.ZipCode).HasColumnName("zip_code").IsRequired().HasMaxLength(20);
            });
            builder.Property(x => x.CartId).HasColumnName("cart_id");
            builder.Property(x => x.PasswordHash).IsRequired().HasMaxLength(255).HasColumnName("password_hash");
            builder.Property(x => x.IsAdmin).HasColumnName("is_admin");
        }
    }
}
