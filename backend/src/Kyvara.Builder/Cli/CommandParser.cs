namespace Kyvara.Builder.Cli;

public sealed class CommandParser
{
    public ParsedCommand Parse(string[] args)
    {
        if (args.Length == 0)
        {
            return new ParsedCommand("help", []);
        }

        var command = args[0];

        var parameters = args
            .Skip(1)
            .ToArray();

        return new ParsedCommand(
            command,
            parameters);
    }
}

public sealed record ParsedCommand(
    string Name,
    string[] Arguments);
