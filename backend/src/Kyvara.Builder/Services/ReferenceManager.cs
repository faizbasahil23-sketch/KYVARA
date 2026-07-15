using System.Diagnostics;
using Kyvara.Builder.Models;

namespace Kyvara.Builder.Services;

public sealed class ReferenceManager
{
    public async Task ApplyAsync(
        string root,
        IEnumerable<ProjectReference> graph)
    {
        foreach(var project in graph)
        {
            foreach(var reference in project.References)
            {
                var projectFile = Path.Combine(
                    root,
                    project.Project,
                    $"{project.Project}.csproj");

                var referenceFile = Path.Combine(
                    root,
                    reference,
                    $"{reference}.csproj");

                if (!File.Exists(projectFile))
                    continue;

                if (!File.Exists(referenceFile))
                    continue;

                await Execute(
                    Path.GetDirectoryName(projectFile)!,
                    $"add \"{projectFile}\" reference \"{referenceFile}\"");
            }
        }
    }

    private static async Task Execute(
        string workingDirectory,
        string arguments)
    {
        var process = new Process();

        process.StartInfo.FileName = "dotnet";
        process.StartInfo.Arguments = arguments;
        process.StartInfo.WorkingDirectory = workingDirectory;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;

        process.Start();

        Console.WriteLine(await process.StandardOutput.ReadToEndAsync());

        var error = await process.StandardError.ReadToEndAsync();

        if(!string.IsNullOrWhiteSpace(error))
            Console.WriteLine(error);

        await process.WaitForExitAsync();

        if(process.ExitCode != 0)
            throw new Exception(arguments);
    }
}
