namespace Kyvara.Builder.Generators.Infrastructure;

public sealed class SeedDataGenerator
{
    public string Generate()
    {
        return """
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
""";
    }
}
