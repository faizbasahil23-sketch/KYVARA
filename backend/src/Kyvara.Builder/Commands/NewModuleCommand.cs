using Spectre.Console;

namespace Kyvara.Builder.Commands;

public sealed class NewModuleCommand : ICommand
{
    public string Name => "new";

    public Task ExecuteAsync(string[] args)
    {
        if(args.Length<2)
        {
            AnsiConsole.MarkupLine("[red]Module name missing.[/]");
            return Task.CompletedTask;
        }

        var module=args[1];

        AnsiConsole.MarkupLine($"[green]Creating module {module}[/]");

        return Task.CompletedTask;
    }
}
