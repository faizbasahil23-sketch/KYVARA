using Kyvara.Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Kyvara.Infrastructure.Persistence.Converters;

public sealed class UserPasswordConverter
    : ValueConverter<UserPassword, string>
{
    public UserPasswordConverter()
        : base(
            password => password.Value,
            value => new UserPassword(value))
    {
    }
}
