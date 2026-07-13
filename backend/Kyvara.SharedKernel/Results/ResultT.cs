namespace Kyvara.SharedKernel.Results;

/// <summary>
/// Represents a generic operation result.
/// </summary>
public sealed class Result<T> : Result
{
    private Result(
        bool isSuccess,
        T? value,
        Error error)
        : base(isSuccess, error)
    {
        Value = value;
    }

    public T? Value { get; }

    public static Result<T> Success(T value)
        => new(true, value, Error.None);

    public static new Result<T> Failure(Error error)
        => new(false, default, error);
}
