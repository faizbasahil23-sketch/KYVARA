namespace Kyvara.Builder.Engine;

public sealed class EnterpriseSolution
{
    private readonly List<string> _projects = new();

    private readonly Dictionary<string,List<string>> _references = new();

    public string Name { get; }

    public EnterpriseSolution(string name)
    {
        Name = name;
    }

    public void AddProject(string project)
    {
        if (!_projects.Contains(project))
            _projects.Add(project);
    }

    public void AddReference(
        string project,
        string reference)
    {
        if (!_references.TryGetValue(project, out var list))
        {
            list = new List<string>();

            _references[project] = list;
        }

        if (!list.Contains(reference))
            list.Add(reference);
    }

    public IReadOnlyList<string> Projects => _projects;

    public IReadOnlyDictionary<string,List<string>> References
        => _references;
}
