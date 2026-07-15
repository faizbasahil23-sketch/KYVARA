using System.Diagnostics;

namespace Kyvara.Builder.Services;

public sealed class ProjectGenerator
{
    public async Task CreateClassLibraryAsync(string root,string projectName)
    {
        await Run(root,$"new classlib -n {projectName} -f net10.0");
    }

    public async Task CreateWebApiAsync(string root,string projectName)
    {
        await Run(root,$"new webapi -n {projectName} -f net10.0");
    }

    public async Task CreateXunitAsync(string root,string projectName)
    {
        await Run(root,$"new xunit -n {projectName} -f net10.0");
    }

    public async Task AddToSolutionAsync(string solution,string project)
    {
        await Run(
            Path.GetDirectoryName(solution)!,
            $"sln \"{solution}\" add \"{project}\"");
    }

    private static async Task Run(string workingDirectory,string arguments)
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

        if (!string.IsNullOrWhiteSpace(error))
            Console.WriteLine(error);

        await process.WaitForExitAsync();

        if (process.ExitCode != 0)
            throw new Exception($"dotnet {arguments} failed.");
    }
}
