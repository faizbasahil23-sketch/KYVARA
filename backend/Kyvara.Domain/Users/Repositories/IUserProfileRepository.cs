using Kyvara.Domain.Users.Entities;

namespace Kyvara.Domain.Users.Repositories;

public interface IUserProfileRepository
{
    Task<UserProfile?> GetAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        UserProfile profile,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        UserProfile profile,
        CancellationToken cancellationToken = default);
}
