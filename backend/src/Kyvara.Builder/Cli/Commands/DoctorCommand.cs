using Kyvara.Builder.Engine;

namespace Kyvara.Builder.Cli.Commands;

public sealed class DoctorCommand : ICommandHandler
{
    private readonly DoctorEngine _doctor;

    public DoctorCommand(
        DoctorEngine doctor)
    {
        _doctor = doctor;
    }

    public string Name => "doctor";

    public int Execute(string[] arguments)
    {
        Console.WriteLine("--------------------------------");
        Console.WriteLine("KYVARA DOCTOR");
        Console.WriteLine("--------------------------------");

        var result = _doctor.Run();

        if (!result.Success)
        {
            Console.WriteLine("Environment check failed.");
            Console.WriteLine(result.Report);

            return 1;
        }

        Console.WriteLine(result.Report);
        Console.WriteLine();
        Console.WriteLine("Environment OK.");

        return 0;
    }
}
