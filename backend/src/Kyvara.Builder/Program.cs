using Kyvara.Builder.Cli;

Console.WriteLine("================================");
Console.WriteLine("      KYVARA Builder CLI");
Console.WriteLine("================================");

var parser = new CommandParser();

var command = parser.Parse(args);

Console.WriteLine($"Command : {command.Name}");

if (command.Arguments.Length > 0)
{
    Console.WriteLine("Arguments:");

    foreach (var argument in command.Arguments)
    {
        Console.WriteLine($" - {argument}");
    }
}
else
{
    Console.WriteLine("No arguments.");
}

return 0;
