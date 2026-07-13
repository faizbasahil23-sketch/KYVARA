namespace Kyvara.SharedKernel.Events;

public abstract class DomainEventCollection
    : IHasDomainEvents
{
    private readonly List<IDomainEvent> _events = new();

    public IReadOnlyCollection<IDomainEvent> DomainEvents
        => _events;

    protected void Raise(IDomainEvent domainEvent)
    {
        _events.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _events.Clear();
    }
}
