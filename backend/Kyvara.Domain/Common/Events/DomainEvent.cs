namespace Kyvara.Domain.Common.Events;

/// <summary>
/// Base Domain Event.
/// </summary>
public abstract record DomainEvent(
    DateTime OccurredOnUtc);
