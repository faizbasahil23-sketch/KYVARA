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
            var projectPath = Path.Combine(
                outputDirectory,
                project);

            Directory.CreateDirectory(projectPath);

            var csproj = Path.Combine(
                projectPath,
                $"{project}.csproj");

            if (!File.Exists(csproj))
            {
                File.WriteAllText(
                    csproj,
"""
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

</Project>
""");
            }
        }
    }
}
