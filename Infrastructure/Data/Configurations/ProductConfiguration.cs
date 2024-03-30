using Domain.Category.ValueObject;
using Domain.Product;
using Domain.Product.ValueObject;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products");

        builder.HasKey(p => p.Id);
        builder
            .Property(p => p.Id)
            .HasConversion(productId => productId.Value, value => ProductId.Create(value))
            .ValueGeneratedNever()
            .HasColumnName("product_id");

        builder.Property(p => p.Name).HasColumnName("name");
        builder.HasIndex(p => p.Name).IsUnique();

        builder.Property(p => p.Description).HasColumnName("description");

        builder
            .Property(p => p.Price)
            .HasConversion(price => price.Value, value => Price.Create(value))
            .HasColumnName("price");

        builder
            .Property(p => p.CategoryId)
            .HasConversion(categoryId => categoryId.Value, value => CategoryId.Create(value))
            .ValueGeneratedNever()
            .HasColumnName("category_id");
        builder.HasIndex(p => p.CategoryId);
    }
}
