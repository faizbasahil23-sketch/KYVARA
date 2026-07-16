namespace {{Namespace}}.ValueObjects;

public sealed record {{ValueObject}}
{
    public string Value { get; }

    public {{ValueObject}}(string value)
    {
        if(string.IsNullOrWhiteSpace(value))
            throw new ArgumentException(nameof(value));

        Value = value;
    }

    public override string ToString()
    {
        return Value;
    }
}
