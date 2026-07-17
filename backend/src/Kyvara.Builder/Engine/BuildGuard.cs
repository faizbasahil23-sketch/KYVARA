namespace Kyvara.Builder.Engine;

public sealed record BuildGuardResult(
    bool Success,
    IReadOnlyList<string> Messages);

public sealed class BuildGuard
{
    public BuildGuardResult Check()
    {
        var messages = new List<string>();

        var requiredFiles = new[]
        {
            "Program.cs",
            "Cli\\CommandParser.cs",
            "Cli\\CommandDispatcher.cs",
            "Engine\\SolutionBuilder.cs",
            "Engine\\ProjectCreator.cs",
            "Engine\\RestoreEngine.cs",
            "Engine\\BuildValidator.cs",
            "Engine\\SolutionVerifier.cs"
        };

        foreach (var file in requiredFiles)
        {
            if (!File.Exists(file))
            {
                messages.Add($"Missing file: {file}");
            }
        }

        return new BuildGuardResult(
            messages.Count == 0,
            messages);
    }
}
