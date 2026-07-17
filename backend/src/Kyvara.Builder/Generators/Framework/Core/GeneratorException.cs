namespace Kyvara.Builder.Generators.Framework.Core;

public class GeneratorException : Exception
{
    public GeneratorException(string message)
        : base(message)
    {
    }

    public GeneratorException(
        string message,
        Exception innerException)
        : base(message, innerException)
    {
    }
}
