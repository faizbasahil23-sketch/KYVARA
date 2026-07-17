namespace Kyvara.Builder.Engine;

public sealed record DoctorResult(
    bool Success,
    string Report);

public sealed class DoctorEngine
{
    public DoctorResult Run()
    {
        var report = $"""
KYVARA Doctor Report
====================
.NET Version : {Environment.Version}
OS           : {Environment.OSVersion}
Machine      : {Environment.MachineName}
Time         : {DateTime.Now}
""";

        return new DoctorResult(
            true,
            report);
    }
}
