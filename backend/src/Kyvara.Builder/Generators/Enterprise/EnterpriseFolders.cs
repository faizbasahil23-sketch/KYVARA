namespace Kyvara.Builder.Generators.Enterprise;

public static class EnterpriseFolders
{
    public static readonly string[] Domain =
    {
        "Entities",
        "ValueObjects",
        "Aggregates",
        "Events",
        "Repositories",
        "Specifications",
        "Enums",
        "Exceptions"
    };

    public static readonly string[] Application =
    {
        "Commands",
        "Queries",
        "Handlers",
        "Validators",
        "DTOs",
        "Interfaces",
        "Mappings",
        "Behaviors"
    };

    public static readonly string[] Infrastructure =
    {
        "Persistence",
        "Repositories",
        "Configurations",
        "Migrations",
        "Services"
    };

    public static readonly string[] Api =
    {
        "Controllers",
        "Endpoints",
        "Middlewares",
        "Filters",
        "OpenApi"
    };

    public static readonly string[] Tests =
    {
        "Fixtures",
        "Builders",
        "Mocks"
    };
}
