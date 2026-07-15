using Kyvara.Builder.Generators;

namespace Kyvara.Builder.Commands;

public sealed class NewModuleCommand : ICommand
{
    public string Name => "new";

    public async Task ExecuteAsync(string[] args)
    {
        if (args.Length < 3)
        {
            Console.WriteLine("Usage: new module <Name>");
            return;
        }

        if (!args[1].Equals("module", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("Unknown new command.");
            return;
        }

        var module = args[2];

        var generator = new ModuleGenerator();

        var output = Path.Combine(
            Directory.GetCurrentDirectory(),
            "..",
            "..",
            "..",
            "Modules");

        Directory.CreateDirectory(output);

        await generator.GenerateAsync(output, module);

        Console.WriteLine();
        Console.WriteLine("================================");
        Console.WriteLine($"Module {module} generated.");
        Console.WriteLine("================================");
        Console.WriteLine();
    }
}