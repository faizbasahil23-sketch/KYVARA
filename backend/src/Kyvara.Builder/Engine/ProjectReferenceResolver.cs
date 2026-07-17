namespace Kyvara.Builder.Engine;

public sealed class ProjectReferenceResolver
{
    private readonly Dictionary<string,List<string>> _references = new();

    public void AddReference(
        string project,
        string reference)
    {
        if (!_references.ContainsKey(project))
            _references[project] = new();

        _references[project].Add(reference);
    }

    public IReadOnlyDictionary<string,List<string>> GetAll()
    {
        return _references;
    }

    public void ConfigureDefaultEnterpriseReferences(
        string solution)
    {
        AddReference(
            $"{solution}.Application",
            $"{solution}.Domain");

        AddReference(
            $"{solution}.Infrastructure",
            $"{solution}.Application");

        AddReference(
            $"{solution}.Api",
            $"{solution}.Application");
    }
}
