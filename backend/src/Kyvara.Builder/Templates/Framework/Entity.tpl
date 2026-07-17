namespace {{Namespace}}.Entities;

public sealed class {{EntityName}}
{
    public Guid Id { get; private set; }

    private {{EntityName}}()
    {
    }

    public static {{EntityName}} Create()
    {
        return new {{EntityName}}
        {
            Id = Guid.NewGuid()
        };
    }
}
