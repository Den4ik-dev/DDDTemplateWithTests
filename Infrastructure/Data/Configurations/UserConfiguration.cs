using Domain.User;
using Domain.User.ValueObject;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(u => u.Id);
        builder
            .Property(u => u.Id)
            .HasConversion(userId => userId.Value, value => UserId.Create(value))
            .ValueGeneratedNever()
            .HasColumnName("user_id");

        builder.Property(u => u.Login).HasColumnName("login");
        builder.HasIndex(u => u.Login).IsUnique();

        builder
            .Property(u => u.Password)
            .HasConversion(password => password.Value, value => Password.Create(value))
            .HasColumnName("password");

        builder
            .Property(u => u.Role)
            .HasConversion(userRole => userRole.Value, value => UserRole.Create(value))
            .HasColumnName("role");

        builder.Property(u => u.RefreshToken).HasColumnName("refresh_token");

        builder.Property(u => u.RefreshTokenExpiryTime).HasColumnName("refresh_token_expiry_time");
    }
}
