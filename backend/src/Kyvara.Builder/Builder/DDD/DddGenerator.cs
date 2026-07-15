namespace Kyvara.Builder.Builder.DDD;

public sealed class DddGenerator
{
    public void Generate(string projectRoot)
    {
        foreach(var folder in DddFolders.Items)
        {
            Directory.CreateDirectory(
                Path.Combine(projectRoot, folder));
        }
    }
}
