using Microsoft.EntityFrameworkCore;
using Kyvara.Domain.Users.Entities;
using Kyvara.Domain.Users.Repositories;
using Kyvara.Infrastructure.Persistence;

namespace Kyvara.Infrastructure.Repositories;

public sealed class UserProfileRepository : IUserProfileRepository
{
    private readonly KyvaraDbContext _db;

    public UserProfileRepository(KyvaraDbContext db)
    {
        _db = db;
    }

    public async Task<UserProfile?> GetAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _db.UserProfiles
            .FirstOrDefaultAsync(
                x => x.UserId == id,
                cancellationToken);
    }

    public async Task AddAsync(
        UserProfile profile,
        CancellationToken cancellationToken = default)
    {
        await _db.UserProfiles.AddAsync(
            profile,
            cancellationToken);

        await _db.SaveChangesAsync(
            cancellationToken);
    }

    public async Task UpdateAsync(
        UserProfile profile,
        CancellationToken cancellationToken = default)
    {
        _db.UserProfiles.Update(profile);

        await _db.SaveChangesAsync(
            cancellationToken);
    }
}
