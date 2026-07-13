namespace Kyvara.Domain.Common.Base;

using Kyvara.Domain.Common.Events;

/// <summary>
/// Base Aggregate Root.
/// </summary>
public abstract class AggregateRoot<TId>
{
    private readonly List<DomainEvent> _events = new();

    public TId Id { get; protected set; } = default!;

    public IReadOnlyCollection<DomainEvent> DomainEvents
        => _events.AsReadOnly();

    protected void Raise(DomainEvent domainEvent)
        => _events.Add(domainEvent);

    public void ClearEvents()
        => _events.Clear();
}
