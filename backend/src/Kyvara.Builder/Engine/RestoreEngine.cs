using System.Diagnostics;

namespace Kyvara.Builder.Engine;

public sealed class RestoreEngine
{
    public bool Restore(string workingDirectory)
    {
        var process = new Process();

        process.StartInfo = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = "restore",
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
