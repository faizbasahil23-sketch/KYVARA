using Kyvara.Domain.Users;
using Kyvara.Domain.Users.ValueObjects;
using Kyvara.Infrastructure.Persistence;

namespace Kyvara.Infrastructure.Persistence.Seed;

public static class DefaultAdminSeeder
{
    public static async Task SeedAsync(
        KyvaraDbContext db)
    {
        if (db.Users.Any())
            return;

        var admin = User.Register(
            new UserEmail("admin@kyvara.id"),
            new UserPassword(
                BCrypt.Net.BCrypt.HashPassword("Admin@123")),
            DateTime.UtcNow);

        db.Users.Add(admin);

        await db.SaveChangesAsync();
    }
}
