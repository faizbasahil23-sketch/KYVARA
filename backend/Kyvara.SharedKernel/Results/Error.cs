namespace Kyvara.SharedKernel.Results;

public sealed record Error(
    string Code,
    string Description)
{
    public static readonly Error None =
        new(string.Empty,string.Empty);

    public static Error Failure(
        string code,
        string description)
        => new(code,description);
}
