using Domain.Category;
using Domain.Category.ValueObject;
using Domain.Product.ValueObject;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("categories");

        builder.HasKey(c => c.Id);
        builder
            .Property(c => c.Id)
            .HasConversion(categoryId => categoryId.Value, value => CategoryId.Create(value))
            .ValueGeneratedNever()
            .HasColumnName("category_id");

        builder.Property(c => c.Name).HasColumnName("name");
        builder.HasIndex(c => c.Name).IsUnique();

        builder.OwnsMany(
            c => c.ProductIds,
            builder =>
            {
                builder.ToTable("category_product_ids");

                builder.Property<int>("id");
                builder.HasKey("id");

                builder.Property(pi => pi.Value).ValueGeneratedNever().HasColumnName("product_id");
                builder.HasIndex(pi => pi.Value);

                builder.WithOwner().HasForeignKey("category_id");
                builder.Property("category_id").IsRequired();
            }
        );
    }
}
