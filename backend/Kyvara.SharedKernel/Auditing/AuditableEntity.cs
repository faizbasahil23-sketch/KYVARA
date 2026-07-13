using Kyvara.SharedKernel.Entities;

namespace Kyvara.SharedKernel.Auditing;

public abstract class AuditableEntity
    : BaseEntity,
      IAuditableEntity
{
    public string? CreatedBy { get; private set; }

    public string? UpdatedBy { get; private set; }

    public void SetCreated(
        DateTime createdAt,
        string? createdBy)
    {
        CreatedAt = createdAt;
        CreatedBy = createdBy;
    }

    public void SetUpdated(
        DateTime updatedAt,
        string? updatedBy)
    {
        UpdatedAt = updatedAt;
        UpdatedBy = updatedBy;
    }
}
