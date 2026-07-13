namespace Kyvara.SharedKernel.Abstractions;

/// <summary>
/// Provides current UTC time.
/// </summary>
public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
