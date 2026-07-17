using Kyvara.Builder.Engine;

namespace Kyvara.Builder.Cli.Commands;

public sealed class NewCommand : ICommandHandler
{
    private readonly EnterpriseScaffolding _scaffolding;

    public NewCommand(
        EnterpriseScaffolding scaffolding)
    {
        _scaffolding = scaffolding;
    }

    public string Name => "new";

    public int Execute(string[] arguments)
    {
        if (arguments.Length == 0)
        {
            Console.WriteLine(
                "Usage: kyvara new <SolutionName>");

            return 1;
        }

        var solutionName = arguments[0];

        var outputDirectory = Path.Combine(
            Directory.GetCurrentDirectory(),
            solutionName);

        Console.WriteLine(
            $"Generating enterprise solution '{solutionName}'...");

        var success = _scaffolding.Generate(
            outputDirectory);

        if (!success)
        {
            Console.WriteLine("Generation failed.");

            return 1;
        }

        Console.WriteLine(
            "Enterprise solution generated successfully.");

        return 0;
    }
}
