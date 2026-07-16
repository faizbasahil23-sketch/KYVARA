namespace Kyvara.Builder.Generators.Infrastructure;

public sealed class RepositoryImplementationGenerator
{
    public string Generate()
    {
        return """
using Microsoft.EntityFrameworkCore;

namespace {{Namespace}}.Persistence.Repositories;

public class Repository<TEntity> : IRepository<TEntity>
    where TEntity : class
{
    protected readonly ApplicationDbContext Context;

    public Repository(ApplicationDbContext context)
    {
        Context = context;
    }

    public async Task<TEntity?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<TEntity>()
            .FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IReadOnlyList<TEntity>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<TEntity>()
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        TEntity entity,
        CancellationToken cancellationToken = default)
    {
        await Context.Set<TEntity>()
            .AddAsync(entity, cancellationToken);
    }

    public Task UpdateAsync(
        TEntity entity,
        CancellationToken cancellationToken = default)
    {
        Context.Set<TEntity>().Update(entity);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(
        TEntity entity,
        CancellationToken cancellationToken = default)
    {
        Context.Set<TEntity>().Remove(entity);

        return Task.CompletedTask;
    }
}
""";
    }
}
