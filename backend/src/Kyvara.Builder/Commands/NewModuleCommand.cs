using Kyvara.Builder.Generators;

namespace Kyvara.Builder.Commands;

public sealed class NewModuleCommand : ICommand
{
    public string Name => "new";

    public async Task ExecuteAsync(string[] args)
    {
        if(args.Length < 2)
        {
            Console.WriteLine("Usage: new module <Name>");
            return;
        }

        if(args[0].ToLower() != "module")
        {
            Console.WriteLine("Unknown new command.");
            return;
        }

        var module = args[1];

        var generator = new ModuleGenerator();

        var output = Path.Combine(
            Directory.GetCurrentDirectory(),
            "..",
            "..",
            "..",
            "Modules");

        Directory.CreateDirectory(output);

        await generator.GenerateAsync(output,module);

        Console.WriteLine($"Module {module} generated.");
    }
}
