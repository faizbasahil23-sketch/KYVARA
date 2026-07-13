using Kyvara.Domain.Users.ValueObjects;

namespace Kyvara.Domain.Users.Repositories;

/// <summary>
/// User repository contract.
/// </summary>
public interface IUserRepository
{
    Task<User?> GetByIdAsync(
        UserId id,
        CancellationToken cancellationToken = default);

    Task<User?> GetByEmailAsync(
        UserEmail email,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        User user,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        User user,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        User user,
        CancellationToken cancellationToken = default);
}
