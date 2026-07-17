namespace Kyvara.Builder.Cli;

public interface ICommandHandler
{
    string Name { get; }

    int Execute(string[] arguments);
}

public sealed class CommandDispatcher
{
    private readonly Dictionary<string, ICommandHandler> _handlers;

    public CommandDispatcher(IEnumerable<ICommandHandler> handlers)
    {
        _handlers = handlers.ToDictionary(
            h => h.Name,
            StringComparer.OrdinalIgnoreCase);
    }

    public int Dispatch(ParsedCommand command)
    {
        if (!_handlers.TryGetValue(command.Name, out var handler))
        {
            Console.WriteLine($"Unknown command: {command.Name}");
            return 1;
        }

        return handler.Execute(command.Arguments);
    }
}
