namespace Kyvara.Builder.Generators.Enterprise;

public sealed class EnterpriseModuleDefinition
{
    public string Name { get; set; } = "";

    public List<string> Projects { get; } =
    [
        "Domain",
        "Application",
        "Infrastructure",
        "Api",
        "Contracts",
        "Shared",
        "UnitTests",
        "IntegrationTests",
        "ArchitectureTests"
    ];
}
