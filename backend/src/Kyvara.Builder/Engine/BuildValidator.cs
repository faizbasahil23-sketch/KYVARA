using System.Diagnostics;

namespace Kyvara.Builder.Engine;

public sealed record BuildResult(
    bool Success,
    int ExitCode,
    string Output,
    string Errors);

public sealed class BuildValidator
{
    public BuildResult Validate(string workingDirectory)
    {
        var process = new Process();

        process.StartInfo = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = "build",
            WorkingDirectory = workingDirectory,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false
        };

        process.Start();

        string output = process.StandardOutput.ReadToEnd();
        string errors = process.StandardError.ReadToEnd();

        process.WaitForExit();

        return new BuildResult(
            process.ExitCode == 0,
            process.ExitCode,
            output,
            errors);
    }
}
