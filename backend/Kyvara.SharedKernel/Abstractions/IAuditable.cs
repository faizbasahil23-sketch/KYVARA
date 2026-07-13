namespace Kyvara.SharedKernel.Abstractions;

/// <summary>
/// Represents an auditable entity.
/// </summary>
public interface IAuditable
{
    DateTime CreatedAt { get; }

    DateTime? UpdatedAt { get; }
}
