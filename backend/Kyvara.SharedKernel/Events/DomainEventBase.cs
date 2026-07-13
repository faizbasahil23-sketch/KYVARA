namespace Kyvara.SharedKernel.Events;

public abstract record DomainEventBase
    : IDomainEvent
{
    public Guid EventId { get; init; }
        = Guid.NewGuid();

    public DateTime OccurredOn { get; init; }
        = DateTime.UtcNow;
}
