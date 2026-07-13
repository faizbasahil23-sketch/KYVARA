using Kyvara.Domain.Users;
using Kyvara.Domain.Users.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kyvara.Infrastructure.Persistence;

/// <summary>
/// Main EF Core DbContext.
/// </summary>
public sealed class KyvaraDbContext : DbContext
{
    public KyvaraDbContext(DbContextOptions<KyvaraDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(KyvaraDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
