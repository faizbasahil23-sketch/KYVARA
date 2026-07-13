namespace Kyvara.SharedKernel.Common;

/// <summary>
/// Smart enumeration.
/// </summary>
public abstract class Enumeration : IComparable
{
    protected Enumeration(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public int Id { get; }

    public string Name { get; }

    public override string ToString()
        => Name;

    public int CompareTo(object? other)
        => Id.CompareTo(((Enumeration)other!).Id);
}
