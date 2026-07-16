namespace {{Namespace}}.Persistence;

public static class SeedData
{
    public static async Task InitializeAsync(
        ApplicationDbContext context)
    {
        if (context.Database.EnsureCreated())
        {
            await context.SaveChangesAsync();
        }
    }
}
