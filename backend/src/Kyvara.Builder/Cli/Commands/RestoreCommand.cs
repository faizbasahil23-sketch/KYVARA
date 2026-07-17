using Kyvara.Builder.Engine;

namespace Kyvara.Builder.Cli.Commands;

public sealed class RestoreCommand : ICommandHandler
{
    private readonly RestoreEngine _restore;

    public RestoreCommand(
        RestoreEngine restore)
    {
        _restore = restore;
    }

    public string Name => "restore";

    public int Execute(string[] arguments)
    {
        var directory = arguments.Length == 0
            ? Directory.GetCurrentDirectory()
            : arguments[0];

        Console.WriteLine("--------------------------------");
        Console.WriteLine("KYVARA RESTORE");
        Console.WriteLine("--------------------------------");
        Console.WriteLine($"Directory : {directory}");
        Console.WriteLine();

        var success = _restore.Restore(directory);

        if (!success)
        {
            Console.WriteLine("RESTORE FAILED");
            return 1;
        }

        Console.WriteLine("RESTORE SUCCESS");

        return 0;
    }
}
