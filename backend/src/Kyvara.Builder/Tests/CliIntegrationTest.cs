using Kyvara.Builder.Cli;
using Kyvara.Builder.Engine;

namespace Kyvara.Builder.Tests;

public sealed class CliIntegrationTest
{
    public bool Run()
    {
        try
        {
            var parser = new CommandParser();

            var parsed = parser.Parse(
                new[] { "build", "SampleSolution" });

            if (parsed.Name != "build")
                return false;

            if (parsed.Arguments.Length != 1)
                return false;

            var registry = new ServiceRegistry();

            registry.Register(parser);

            if (!registry.IsRegistered<CommandParser>())
                return false;

            return true;
        }
        catch
        {
            return false;
        }
    }
}
