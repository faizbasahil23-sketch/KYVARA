using Spectre.Console;
using Kyvara.Builder.Commands;

AnsiConsole.Write(
    new FigletText("KYVARA")
        .Centered()
        .Color(Color.Cyan));

if(args.Length>=3 &&
   args[0].ToLower()=="new" &&
   args[1].ToLower()=="module")
{
    await new NewModuleCommand()
        .ExecuteAsync(args[2]);

    return;
}

AnsiConsole.MarkupLine("[yellow]Available Commands[/]");

AnsiConsole.MarkupLine("doctor");

AnsiConsole.MarkupLine("build");

AnsiConsole.MarkupLine("repair");

AnsiConsole.MarkupLine("new module <Name>");

AnsiConsole.MarkupLine("publish");
