namespace Kyvara.SharedKernel.Events;

public interface IDomainEventDispatcher
{
    Task DispatchAsync(
        IDomainEvent domainEvent,
        CancellationToken cancellationToken = default);

    Task DispatchAsync(
        IEnumerable<IDomainEvent> events,
        CancellationToken cancellationToken = default);
}
