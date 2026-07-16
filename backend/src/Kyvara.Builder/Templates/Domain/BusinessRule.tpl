namespace {{Namespace}}.Rules;

public interface IBusinessRule
{
    bool IsBroken();

    string Message { get; }
}
