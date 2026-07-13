using Kyvara.Domain.Users;
using Kyvara.Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kyvara.Infrastructure.Configurations;

/// <summary>
/// EF Core configuration for User aggregate.
/// </summary>
public sealed class UserConfiguration
    : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        // ==========================
        // Primary Key
        // ==========================

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion<UserIdConverter>()
            .ValueGeneratedNever();

        // ==========================
        // Email
        // ==========================

        builder.Property(x => x.Email)
            .HasConversion<UserEmailConverter>()
            .HasMaxLength(320)
            .IsRequired();

        builder.HasIndex(x => x.Email)
            .IsUnique();

        // ==========================
        // Password
        // ==========================

        builder.Property(x => x.Password)
            .HasConversion<UserPasswordConverter>()
            .HasMaxLength(500)
            .IsRequired();

        // ==========================
        // RegisteredAt
        // ==========================

        builder.Property(x => x.RegisteredAt)
            .IsRequired();
    }
}
