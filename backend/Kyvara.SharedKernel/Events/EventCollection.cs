using Kyvara.SharedKernel.Abstractions;

namespace Kyvara.SharedKernel.Events;

/// <summary>
/// Collection of domain events.
/// </summary>
public sealed class EventCollection : List<IDomainEvent>
{
}
