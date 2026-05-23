using E_commerce_API.src.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_commerce_API.src.Infrastructure.Configuration
{
    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.ToTable("carts");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");
            builder.Property(x => x.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP").HasColumnName("created_at");
            builder.Property(x => x.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP").HasColumnName("updated_at");
            builder.Property(x => x.UserId).HasColumnName("user_id");
            builder.HasOne(x => x.User).WithOne(x => x.Cart).HasForeignKey<Cart>(x => x.UserId);
            builder.HasIndex(x => x.UserId).IsUnique();
        }
    }
}
