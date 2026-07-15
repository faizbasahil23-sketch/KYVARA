namespace Kyvara.Builder.Models;

public sealed class ProjectReference
{
    public string Project { get; set; } = "";

    public List<string> References { get; } = new();
}
