namespace Kyvara.SharedKernel.Abstractions;

/// <summary>
/// Represents a soft deletable entity.
/// </summary>
public interface ISoftDelete
{
    bool IsDeleted { get; }

    DateTime? DeletedAt { get; }
}
