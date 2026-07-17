namespace Kyvara.Builder.Engine;

public sealed class GeneratorRegistry
{
    private readonly Dictionary<Type, object> _generators = new();

    public void Register<T>(T generator)
        where T : class
    {
        ArgumentNullException.ThrowIfNull(generator);

        _generators[typeof(T)] = generator;
    }

    public T Resolve<T>()
        where T : class
    {
        if (_generators.TryGetValue(typeof(T), out var generator))
        {
            return (T)generator;
        }

        throw new InvalidOperationException(
            $"Generator '{typeof(T).Name}' is not registered.");
    }

    public bool IsRegistered<T>()
        where T : class
    {
        return _generators.ContainsKey(typeof(T));
    }
}
