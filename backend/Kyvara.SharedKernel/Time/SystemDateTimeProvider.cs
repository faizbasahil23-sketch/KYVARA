namespace Kyvara.SharedKernel.Time;

public sealed class SystemDateTimeProvider
    : IDateTimeProvider
{
    public DateTime UtcNow
        => DateTime.UtcNow;
}
