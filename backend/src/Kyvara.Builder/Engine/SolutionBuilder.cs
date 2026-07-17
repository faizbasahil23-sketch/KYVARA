namespace Kyvara.Builder.Engine;

public sealed class SolutionBuilder
{
    public string SolutionName { get; }

    public SolutionBuilder(string solutionName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(solutionName);

        SolutionName = solutionName;
    }

    public string GetSolutionFile()
    {
        return $"{SolutionName}.slnx";
    }

    public string GetSourceDirectory()
    {
        return Path.Combine(SolutionName, "src");
    }

    public string GetTestsDirectory()
    {
        return Path.Combine(SolutionName, "tests");
    }

    public IReadOnlyList<string> GetProjects()
    {
        return
        [
            $"{SolutionName}.Api",
            $"{SolutionName}.Application",
            $"{SolutionName}.Contracts",
            $"{SolutionName}.Domain",
            $"{SolutionName}.Infrastructure"
        ];
    }

    public IReadOnlyList<string> GetTestProjects()
    {
        return
        [
            $"{SolutionName}.UnitTests",
            $"{SolutionName}.IntegrationTests",
            $"{SolutionName}.ArchitectureTests"
        ];
    }
}
