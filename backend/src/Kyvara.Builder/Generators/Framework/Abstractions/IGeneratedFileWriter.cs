namespace Kyvara.Builder.Generators.Framework.Abstractions;

public interface IGeneratedFileWriter
{
    Task WriteAsync(
        string outputDirectory,
        string relativePath,
        string content,
        bool overwrite,
        CancellationToken cancellationToken = default);
}
