using Kyvara.Builder.Engine;

namespace Kyvara.Builder.Cli.Commands;

public sealed class VerifyCommand : ICommandHandler
{
    private readonly SolutionComposer _composer;
    private readonly SolutionVerifier _verifier;

    public VerifyCommand(
        SolutionComposer composer,
        SolutionVerifier verifier)
    {
        _composer = composer;
        _verifier = verifier;
    }

    public string Name => "verify";

    public int Execute(string[] arguments)
    {
        Console.WriteLine("--------------------------------");
        Console.WriteLine("KYVARA VERIFY");
        Console.WriteLine("--------------------------------");

        var solution = _composer.Compose();

        var result = _verifier.Verify(solution);

        if (!result.Success)
        {
            Console.WriteLine("Verification failed.");
            Console.WriteLine();

            foreach (var error in result.Errors)
            {
                Console.WriteLine($" - {error}");
            }

            return 1;
        }

        Console.WriteLine("Verification succeeded.");

        return 0;
    }
}
