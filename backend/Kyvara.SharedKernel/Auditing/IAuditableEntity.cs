namespace Kyvara.SharedKernel.Auditing;

public interface IAuditableEntity
{
    DateTime CreatedAt { get; }

    string? CreatedBy { get; }

    DateTime? UpdatedAt { get; }

    string? UpdatedBy { get; }

    void SetCreated(
        DateTime createdAt,
        string? createdBy);

    void SetUpdated(
        DateTime updatedAt,
        string? updatedBy);
}
