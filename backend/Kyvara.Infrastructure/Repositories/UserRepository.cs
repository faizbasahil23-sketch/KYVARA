using Microsoft.EntityFrameworkCore;
using Kyvara.Domain.Users;
using Kyvara.Domain.Users.Repositories;
using Kyvara.Domain.Users.ValueObjects;
using Kyvara.Infrastructure.Persistence;

namespace Kyvara.Infrastructure.Repositories;

public sealed class UserRepository : IUserRepository
{
    private readonly KyvaraDbContext _db;

    public UserRepository(KyvaraDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(
        User user,
        CancellationToken cancellationToken = default)
    {
        await _db.Users.AddAsync(user, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task<User?> GetByIdAsync(
        UserId id,
        CancellationToken cancellationToken = default)
    {
        return await _db.Users.FirstOrDefaultAsync(
            x => x.Id == id,
            cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(
        UserEmail email,
        CancellationToken cancellationToken = default)
    {
        return await _db.Users.FirstOrDefaultAsync(
            x => x.Email == email,
            cancellationToken);
    }

    public async Task UpdateAsync(
        User user,
        CancellationToken cancellationToken = default)
    {
        _db.Users.Update(user);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        User user,
        CancellationToken cancellationToken = default)
    {
        _db.Users.Remove(user);
        await _db.SaveChangesAsync(cancellationToken);
    }
}
