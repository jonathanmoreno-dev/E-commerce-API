using E_commerce_API.src.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_commerce_API.src.Infrastructure.Configuration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(x => x.CategoryId);
            builder.ComplexProperty(x => x.Name, p => p.Property(v => v.Value).HasColumnName("name").IsRequired().HasMaxLength(100));
            builder.ComplexProperty(x => x.Description, p => p.Property(v => v.Value).HasColumnName("description").IsRequired().HasMaxLength(400));
        }
    }
}
