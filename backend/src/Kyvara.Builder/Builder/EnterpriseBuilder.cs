namespace Kyvara.Builder.Builder;

public sealed class EnterpriseBuilder
{
    public string Name { get; }

    public EnterpriseBuilder(string name)
    {
        Name=name;
    }

    public async Task BuildAsync()
    {
        Console.WriteLine($"Generating Enterprise Solution : {Name}");

        await Task.CompletedTask;
    }
}
