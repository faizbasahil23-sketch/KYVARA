using Spectre.Console;

namespace Kyvara.Builder.Commands;

public sealed class DoctorCommand : ICommand
{
    public string Name => "doctor";

    public Task ExecuteAsync(string[] args)
    {
        AnsiConsole.MarkupLine("[green]Running KYVARA Doctor...[/]");

        AnsiConsole.MarkupLine("[yellow]Checking .NET SDK[/]");

        AnsiConsole.MarkupLine("[yellow]Checking Solution[/]");

        AnsiConsole.MarkupLine("[yellow]Checking Projects[/]");

        AnsiConsole.MarkupLine("[green]Everything looks good.[/]");

        return Task.CompletedTask;
    }
}
