using Kyvara.Builder.Commands;
using Spectre.Console;

var commands = new Dictionary<string, ICommand>(StringComparer.OrdinalIgnoreCase)
{
    ["doctor"] = new DoctorCommand(),
    ["build"] = new BuildCommand(),
    ["repair"] = new RepairCommand(),
    ["new"] = new NewModuleCommand()
};

AnsiConsole.Write(
    new FigletText("KYVARA")
        .Centered()
        .Color(Color.Cyan));

AnsiConsole.MarkupLine("[green]Enterprise Builder v2[/]");
AnsiConsole.WriteLine();

if (args.Length == 0)
{
    AnsiConsole.MarkupLine("[yellow]Available Commands[/]");
    foreach (var command in commands.Keys.OrderBy(x => x))
    {
        AnsiConsole.MarkupLine($"  • {command}");
    }

    return;
}

var commandName = args[0];

if (!commands.TryGetValue(commandName, out var commandHandler))
{
    AnsiConsole.MarkupLine($"[red]Unknown command:[/] {commandName}");
    return;
}

await commandHandler.ExecuteAsync(args);