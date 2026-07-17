using System.Diagnostics;

namespace Kyvara.Builder.Engine;

public sealed class BuildEngine
{
    public bool Build(string workingDirectory)
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

        process.WaitForExit();

        return process.ExitCode == 0;
    }
}
