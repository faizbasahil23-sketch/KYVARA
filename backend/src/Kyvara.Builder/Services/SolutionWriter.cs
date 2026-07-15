using Kyvara.Builder.Models;

namespace Kyvara.Builder.Services;

public sealed class SolutionWriter
{
    private readonly SolutionManager _solution = new();
    private readonly ReferenceManager _references = new();

    public async Task BuildAsync(
        string root,
        string solutionFile,
        IEnumerable<ProjectReference> graph)
    {
        if (!File.Exists(solutionFile))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(solutionFile)!);

            File.WriteAllText(solutionFile, "");
        }

        foreach (var project in graph)
        {
            var projectFile = Path.Combine(
                root,
                project.Project,
                $"{project.Project}.csproj");

            if (!File.Exists(projectFile))
                continue;

            await _solution.AddProjectAsync(
                solutionFile,
                projectFile);
        }

        await _references.ApplyAsync(root, graph);
    }
}
