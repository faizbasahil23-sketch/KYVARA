namespace Kyvara.SharedKernel.Repositories;

public interface IReadRepository<TEntity>
    where TEntity : class
{
    Task<TEntity?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<List<TEntity>> ListAsync(
        CancellationToken cancellationToken = default);
}
