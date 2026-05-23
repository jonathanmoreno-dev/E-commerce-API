using E_commerce_API.src.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_commerce_API.src.Infrastructure.Configuration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("orders");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");
            builder.Property(x => x.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP").HasColumnName("created_at");
            builder.Property(x => x.Status).HasConversion<string>().HasColumnName("status");
            builder.Property(x => x.UserId).HasColumnName("user_id");
            builder.HasOne(x => x.User).WithMany(y => y.Orders).HasForeignKey(x => x.UserId);
            builder.HasIndex(x => x.UserId);
        }
    }
}
