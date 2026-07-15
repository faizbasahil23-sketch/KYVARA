using Kyvara.Builder.Generators;
using Kyvara.Builder.Models;

namespace Kyvara.Builder.Services;

public sealed class EnterpriseBuilder
{
    private readonly ModuleGenerator _moduleGenerator = new();
    private readonly ReferenceGraph _graph = new();
    private readonly SolutionWriter _writer = new();

    public async Task GenerateAsync(
        string root,
        string module)
    {
        Console.WriteLine("--------------------------------");
        Console.WriteLine("KYVARA Enterprise Builder");
        Console.WriteLine("--------------------------------");

        Console.WriteLine("Generating projects...");
        await _moduleGenerator.GenerateAsync(root,module);

        Console.WriteLine("Building dependency graph...");
        var references=_graph.Build(module);

        Console.WriteLine("Generating solution...");

        var solution=
            Path.Combine(root,$"{module}.slnx");

        await _writer.BuildAsync(
            root,
            solution,
            references);

        Console.WriteLine();

        Console.WriteLine("DONE");

        Console.WriteLine(solution);
    }
}
