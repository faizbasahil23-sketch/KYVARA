using System.IO;

namespace Kyvara.Builder.Engine;

public sealed class ProjectCreator
{
    public void Create(
        EnterpriseSolution solution,
        string outputDirectory)
    {
        Directory.CreateDirectory(outputDirectory);

        foreach (var project in solution.Projects)
        {
            var path = Path.Combine(
                outputDirectory,
                project);

            Directory.CreateDirectory(path);
        }
    }
}
