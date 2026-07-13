namespace Kyvara.SharedKernel.Abstractions;

/// <summary>
/// Represents a domain event.
/// </summary>
public interface IDomainEvent
{
    Guid Id { get; }

    DateTime OccurredOnUtc { get; }
}
