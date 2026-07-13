namespace Kyvara.SharedKernel.Time;

public interface IClock
{
    DateTime UtcNow { get; }
}
