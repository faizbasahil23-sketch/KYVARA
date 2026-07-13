using Kyvara.Domain.Users.ValueObjects;
using Kyvara.SharedKernel.Events;

namespace Kyvara.Domain.Users.Events;

/// <summary>
/// Raised when a new user is registered.
/// </summary>
public sealed record UserRegisteredDomainEvent(
    UserId UserId,
    UserEmail Email)
    : DomainEvent;
