using Kyvara.Builder.Generators;

namespace Kyvara.Builder.Commands;

public sealed class NewModuleCommand
{
    public async Task ExecuteAsync(string module)
    {
        var generator = new ModuleGenerator();

        var output = Path.Combine(
            Directory.GetCurrentDirectory(),
            "..",
            "..",
            "..",
            "Modules");

        Directory.CreateDirectory(output);

        await generator.GenerateAsync(output,module);

        Console.WriteLine();

        Console.WriteLine("================================");

        Console.WriteLine($"Module {module} generated.");

        Console.WriteLine("================================");

        Console.WriteLine();
    }
}
