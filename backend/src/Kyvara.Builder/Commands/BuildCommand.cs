using Spectre.Console;

namespace Kyvara.Builder.Commands;

public sealed class BuildCommand : ICommand
{
    public string Name => "build";

    public Task ExecuteAsync(string[] args)
    {
        AnsiConsole.MarkupLine("[green]Building KYVARA...[/]");

        return Task.CompletedTask;
    }
}
