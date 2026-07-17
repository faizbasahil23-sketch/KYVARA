namespace Kyvara.Builder.Engine;

public sealed record DependencyValidationResult(
    bool Success,
    IReadOnlyList<string> MissingTypes);

public sealed class DependencyValidator
{
    private readonly string[] _requiredTypes =
    [
        "SolutionBuilder",
        "ProjectCreator",
        "RestoreEngine",
        "BuildValidator",
        "SolutionVerifier",
        "EnterpriseScaffolding",
        "DoctorEngine",
        "CommandParser",
        "CommandDispatcher"
    ];

    public DependencyValidationResult Validate()
    {
        var missing = new List<string>();

        foreach (var typeName in _requiredTypes)
        {
            var found = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Any(t => t.Name == typeName);

            if (!found)
            {
                missing.Add(typeName);
            }
        }

        return new DependencyValidationResult(
            missing.Count == 0,
            missing);
    }
}
