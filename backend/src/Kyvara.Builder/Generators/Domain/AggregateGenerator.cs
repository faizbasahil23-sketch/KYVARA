namespace Kyvara.Builder.Generators.Domain;

public sealed class AggregateGenerator
{
    public string Generate()
    {
        return """
namespace {{Namespace}}.Aggregates;

public abstract class AggregateRoot
{
    public Guid Id { get; protected set; }

    private readonly List<object> _events = new();

    public IReadOnlyCollection<object> DomainEvents
        => _events.AsReadOnly();

    protected void Raise(object @event)
    {
        _events.Add(@event);
    }

    public void ClearEvents()
    {
        _events.Clear();
    }
}
""";
    }
}
