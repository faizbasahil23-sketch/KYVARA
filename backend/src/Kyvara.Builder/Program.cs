using Spectre.Console;

AnsiConsole.Write(
    new FigletText("KYVARA")
        .Centered()
        .Color(Color.Cyan));

AnsiConsole.MarkupLine("[green]Enterprise Builder v2[/]");

AnsiConsole.MarkupLine("");

AnsiConsole.MarkupLine("[yellow]Available Commands[/]");

AnsiConsole.MarkupLine("  doctor");

AnsiConsole.MarkupLine("  build");

AnsiConsole.MarkupLine("  repair");

AnsiConsole.MarkupLine("  new module <Name>");

AnsiConsole.MarkupLine("  publish");
