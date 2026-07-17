namespace Kyvara.Builder.Engine;

public sealed class FileWriter
{
    public void Write(
        string filePath,
        string content)
    {
        var directory = Path.GetDirectoryName(filePath);

        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        File.WriteAllText(filePath, content);
    }

    public bool Exists(string filePath)
    {
        return File.Exists(filePath);
    }

    public void Delete(string filePath)
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }
}
