namespace Kyvara.Builder.Engine;

public sealed class ProjectWriter
{
    private readonly FolderWriter _folderWriter = new();
    private readonly FileWriter _fileWriter = new();

    public void WriteProject(
        string rootPath,
        Dictionary<string,string> files)
    {
        foreach (var file in files)
        {
            var fullPath = Path.Combine(rootPath, file.Key);

            var directory = Path.GetDirectoryName(fullPath);

            if (!string.IsNullOrWhiteSpace(directory))
            {
                _folderWriter.Create(directory);
            }

            _fileWriter.Write(fullPath, file.Value);
        }
    }
}
