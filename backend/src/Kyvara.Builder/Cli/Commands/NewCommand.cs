namespace Kyvara.Builder.Cli.Commands;

public sealed class NewCommand : ICommandHandler
{
    public string Name => "new";

    public int Execute(string[] arguments)
    {
        if (arguments.Length == 0)
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("  kyvara new <SolutionName>");
            return 1;
        }

        var solutionName = arguments[0];

        Console.WriteLine("--------------------------------");
        Console.WriteLine("KYVARA Enterprise Builder");
        Console.WriteLine("--------------------------------");
        Console.WriteLine($"Creating solution : {solutionName}");
        Console.WriteLine("Status            : OK");
        Console.WriteLine("--------------------------------");

        return 0;
    }
}
