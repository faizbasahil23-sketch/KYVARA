using Kyvara.Builder.Engine;

namespace Kyvara.Builder.Cli.Commands;

public sealed class BuildCommand : ICommandHandler
{
    private readonly BuildValidator _validator;

    public BuildCommand(
        BuildValidator validator)
    {
        _validator = validator;
    }

    public string Name => "build";

    public int Execute(string[] arguments)
    {
        var directory = arguments.Length == 0
            ? Directory.GetCurrentDirectory()
            : arguments[0];

        Console.WriteLine("--------------------------------");
        Console.WriteLine("KYVARA BUILD");
        Console.WriteLine("--------------------------------");
        Console.WriteLine($"Directory : {directory}");
        Console.WriteLine();

        var result = _validator.Validate(directory);

        if (!result.Success)
        {
            Console.WriteLine("BUILD FAILED");
            Console.WriteLine(result.Errors);

            return 1;
        }

        Console.WriteLine("BUILD SUCCESS");

        return 0;
    }
}
