namespace Kyvara.Builder.Engine;

public sealed class FolderWriter
{
    public void Create(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    public bool Exists(string path)
    {
        return Directory.Exists(path);
    }

    public void Delete(string path)
    {
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }
    }

    public void Ensure(params string[] folders)
    {
        foreach (var folder in folders)
        {
            Create(folder);
        }
    }
}
