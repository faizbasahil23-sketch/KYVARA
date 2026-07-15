using Spectre.Console;

namespace Kyvara.Builder.Commands;

public sealed class RepairCommand : ICommand
{
    public string Name => "repair";

    public Task ExecuteAsync(string[] args)
    {
        AnsiConsole.MarkupLine("[yellow]Repairing Solution...[/]");

        return Task.CompletedTask;
    }
}
