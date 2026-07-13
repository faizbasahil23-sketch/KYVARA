using Kyvara.Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Kyvara.Infrastructure.Persistence.Converters;

public sealed class UserEmailConverter
    : ValueConverter<UserEmail, string>
{
    public UserEmailConverter()
        : base(
            email => email.Value,
            value => new UserEmail(value))
    {
    }
}
