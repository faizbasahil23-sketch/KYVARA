namespace Kyvara.SharedKernel.Events;

public interface IDomainEvent
{
    Guid EventId { get; }

    DateTime OccurredOn { get; }
}
