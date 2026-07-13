using Kyvara.Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Kyvara.Infrastructure.Persistence.Converters;

public sealed class UserIdConverter
    : ValueConverter<UserId, Guid>
{
    public UserIdConverter()
        : base(
            id => id.Value,
            value => new UserId(value))
    {
    }
}
