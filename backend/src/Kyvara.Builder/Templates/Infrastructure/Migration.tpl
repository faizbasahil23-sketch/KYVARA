using Microsoft.EntityFrameworkCore;

namespace {{Namespace}}.Persistence;

public static class MigrationRunner
{
    public static async Task ApplyMigrationsAsync(
        ApplicationDbContext context)
    {
        await context.Database.MigrateAsync();
    }
}
