namespace Kyvara.Builder.Generators.Infrastructure;

public sealed class DbContextGenerator
{
    public string Generate()
    {
        return """
using Microsoft.EntityFrameworkCore;

namespace {{Namespace}}.Persistence;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(
        ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(
            typeof(ApplicationDbContext).Assembly);
    }
}
""";
    }
}
