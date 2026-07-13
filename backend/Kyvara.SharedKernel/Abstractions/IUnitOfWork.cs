namespace Kyvara.SharedKernel.Abstractions;

/// <summary>
/// Represents a unit of work.
/// </summary>
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
