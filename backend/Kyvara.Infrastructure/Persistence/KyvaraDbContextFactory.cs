using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Kyvara.Infrastructure.Persistence;

public sealed class KyvaraDbContextFactory
    : IDesignTimeDbContextFactory<KyvaraDbContext>
{
    public KyvaraDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<KyvaraDbContext>();

        builder.UseSqlite(
            "Data Source=kyvara.db");

        return new KyvaraDbContext(builder.Options);
    }
}
