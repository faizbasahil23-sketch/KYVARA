using Kyvara.Domain.Users.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kyvara.Infrastructure.Persistence.Configurations;

public sealed class UserProfileConfiguration
    : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.ToTable("UserProfiles");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.FullName)
            .HasMaxLength(200);

        builder.Property(x => x.Bio)
            .HasMaxLength(1000);

        builder.Property(x => x.AvatarUrl)
            .HasMaxLength(500);

        builder.Property(x => x.TimeZone)
            .HasMaxLength(100);

        builder.Property(x => x.Language)
            .HasMaxLength(20);

        builder.OwnsOne(x => x.Username, b =>
        {
            b.Property(p => p.Value)
                .HasColumnName("Username")
                .HasMaxLength(100);
        });
    }
}
