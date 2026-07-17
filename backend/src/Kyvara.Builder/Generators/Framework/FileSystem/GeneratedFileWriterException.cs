namespace Kyvara.Builder.Generators.Framework.FileSystem;

public sealed class GeneratedFileWriterException : Exception
{
    public GeneratedFileWriterException(string message)
        : base(message)
    {
    }

    public GeneratedFileWriterException(
        string message,
        Exception innerException)
        : base(message, innerException)
    {
    }
}
