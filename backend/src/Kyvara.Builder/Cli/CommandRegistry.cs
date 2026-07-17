namespace Kyvara.Builder.Cli;

public static class CommandRegistry
{
    public static readonly string[] Commands =
    [
        "new",
        "build",
        "restore",
        "verify",
        "doctor"
    ];

    public static bool Exists(string command)
    {
        return Commands.Contains(
            command,
            StringComparer.OrdinalIgnoreCase);
    }
}
