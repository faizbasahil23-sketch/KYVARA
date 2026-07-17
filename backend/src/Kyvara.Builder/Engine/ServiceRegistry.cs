namespace Kyvara.Builder.Engine;

public sealed class ServiceRegistry
{
    private readonly Dictionary<Type, object> _services = new();

    public void Register<TService>(TService instance)
        where TService : class
    {
        ArgumentNullException.ThrowIfNull(instance);

        _services[typeof(TService)] = instance;
    }

    public TService Resolve<TService>()
        where TService : class
    {
        if (_services.TryGetValue(typeof(TService), out var service))
        {
            return (TService)service;
        }

        throw new InvalidOperationException(
            $"Service not registered: {typeof(TService).FullName}");
    }

    public bool IsRegistered<TService>()
        where TService : class
    {
        return _services.ContainsKey(typeof(TService));
    }

    public IReadOnlyCollection<Type> RegisteredServices =>
        _services.Keys.ToArray();
}
