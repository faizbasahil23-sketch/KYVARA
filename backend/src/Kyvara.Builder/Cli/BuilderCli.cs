using Kyvara.Builder.Engine;

namespace Kyvara.Builder.Cli;

public sealed class BuilderCli
{
    private readonly BuilderEngine _engine = new();

    public void Run(string[] args)
    {
        if (args.Length < 3)
        {
            Console.WriteLine(
                "Usage: new module <ModuleName>");
            return;
        }

        if (args[0].Equals("new", StringComparison.OrdinalIgnoreCase)
            && args[1].Equals("module", StringComparison.OrdinalIgnoreCase))
        {
            var module = args[2];

            _engine.BuildModule(
                Path.Combine(Environment.CurrentDirectory, module),
                module,
                $"Kyvara.Modules.{module}");

            Console.WriteLine($"Module '{module}' created successfully.");
            return;
        }

        Console.WriteLine("Unknown command.");
    }
}
