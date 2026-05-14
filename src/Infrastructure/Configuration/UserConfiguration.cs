using E_commerce_API.src.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_commerce_API.src.Infrastructure.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.UserId);
            builder.HasIndex(x => x.Email).IsUnique();

            builder.ComplexProperty(x => x.FullName, p => p.Property(v => v.Value).HasColumnName("FullName").IsRequired().HasMaxLength(150));
            builder.ComplexProperty(x => x.Email, p => p.Property(v => v.Value).HasColumnName("Email").IsRequired().HasMaxLength(255));
            builder.ComplexProperty(x => x.PhoneNumber, p => p.Property(v => v.Value).HasColumnName("PhoneNumber").IsRequired().HasMaxLength(50));

            builder.Property(x => x.PasswordHash).IsRequired().HasMaxLength(255);
            builder.Property(x => x.IsAdmin);
        }
    }
}
