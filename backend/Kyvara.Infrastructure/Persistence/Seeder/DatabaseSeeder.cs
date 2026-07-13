using Kyvara.Application.Authentication.Interfaces;
using Kyvara.Domain.Users;
using Kyvara.Domain.Users.ValueObjects;
using Kyvara.Infrastructure.Persistence;

namespace Kyvara.Infrastructure.Persistence.Seeder;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(
        KyvaraDbContext db,
        IPasswordHasher hasher)
    {
        if (db.Users.Any())
            return;

        var user =
            User.Register(
                new UserEmail("admin@kyvara.id"),
                new UserPassword(hasher.Hash("Admin123!")),
                DateTime.UtcNow);

        db.Users.Add(user);

        await db.SaveChangesAsync();
    }
}
