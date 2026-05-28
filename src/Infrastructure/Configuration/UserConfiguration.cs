using E_commerce_API.src.Domain.Entities;
using E_commerce_API.src.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_commerce_API.src.Infrastructure.Configuration
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

            builder.Property(x => x.PasswordHash).IsRequired().HasMaxLength(255).HasColumnName("password_hash");
            builder.Property(x => x.IsAdmin).HasColumnName("is_admin");
        }
    }
}
