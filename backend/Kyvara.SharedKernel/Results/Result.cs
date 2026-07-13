namespace Kyvara.SharedKernel.Results;

public class Result
{
    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public Error Error { get; }

    protected Result(
        bool success,
        Error error)
    {
        IsSuccess=success;
        Error=error;
    }

    public static Result Success()
    {
        return new Result(
            true,
            Error.None);
    }

    public static Result Failure(
        Error error)
    {
        return new Result(
            false,
            error);
    }
}
