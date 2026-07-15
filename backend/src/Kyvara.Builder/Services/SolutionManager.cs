using System.Diagnostics;

namespace Kyvara.Builder.Services;

public sealed class SolutionManager
{
    public async Task AddProjectAsync(string solution,string project)
    {
        var args = $"sln \"{solution}\" add \"{project}\"";

        await Execute(
            Path.GetDirectoryName(solution)!,
            args);
    }

    public async Task AddReferenceAsync(string project,string reference)
    {
        var args = $"add \"{project}\" reference \"{reference}\"";

        await Execute(
            Path.GetDirectoryName(project)!,
            args);
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
            throw new Exception($"dotnet {arguments} failed.");
    }
}
