using E_commerce_API.src.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_commerce_API.src.Infrastructure.Data.Configuration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.OrderId);
            builder.Property(x => x.TotalAmount).HasPrecision(18, 2);
            builder.Property(x => x.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Property(x => x.Status).HasConversion<string>();
            builder.HasOne(x => x.User).WithMany(y => y.Orders).HasForeignKey(x => x.UserId);
            builder.HasIndex(x => x.UserId).IsUnique();
        }
    }
}
