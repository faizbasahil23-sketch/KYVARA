namespace Kyvara.SharedKernel.Events;

public sealed class DomainEventDispatcher
    : IDomainEventDispatcher
{
    public async Task DispatchAsync(
        IDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;
    }

    public async Task DispatchAsync(
        IEnumerable<IDomainEvent> events,
        CancellationToken cancellationToken = default)
    {
        foreach(var e in events)
        {
            await DispatchAsync(e,cancellationToken);
        }
    }
}
